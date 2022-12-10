using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel;
using NLog;
using NLog.LayoutRenderers.Wrappers;

namespace HelloHome.Central.Hub.NodeBridge
{
    public class NodeOtaFirmwareUploader
    {
        private readonly IByteStream _byteStream;
        private readonly ILogger _logger;

        public NodeOtaFirmwareUploader(IByteStream byteStream, ILogger logger)
        {
            _byteStream = byteStream;
            _logger = logger;
        }

        const int LINEPERPACKET = 3;

        public bool UpdateNode(string firmware, int nodeId)
        {
            var sw = Stopwatch.StartNew();
            if (WaitForTargetSet(nodeId))
            {
                _logger.Trace("TARGET SET OK");
            }
            else
            {
                _logger.Warn("TARGET SET FAIL, exiting...");
                return false;
            }

            var handshakeResponse = WaitForHandshake(false);
            if (handshakeResponse == HANDSHAKE_OK)
            {
                var content = File.ReadAllLines(firmware);
                var seq = 0;
                var packCounter = 0;
                while (seq < content.Length)
                {
                    var currentLine = content[seq].Trim();
                    var isEoF = content[seq].Trim() ==
                                ":00000001FF"; //this should be the last line in any valid intel HEX file
                    var result = -1;
                    var bundledLines = 1;
                    if (!isEoF)
                    {
                        var hexDataToSend = currentLine;

                        if (LINEPERPACKET > 1 && currentLine.Substring(7, 2) == "00")
                        {
                            var nextLine = content[seq + 1].Trim();
                            if (nextLine != ":00000001FF" & nextLine.Substring(7, 2) == "00")
                            {
                                var checksum = int.Parse(currentLine.Substring(currentLine.Length - 2, 2),
                                                   NumberStyles.HexNumber)
                                               + int.Parse(nextLine.Substring(nextLine.Length - 2, 2),
                                                   NumberStyles.HexNumber)
                                               + int.Parse(nextLine.Substring(3, 2), NumberStyles.HexNumber)
                                               + int.Parse(nextLine.Substring(5, 2), NumberStyles.HexNumber);
                                var addresseByte = int.Parse(currentLine.Substring(1, 2), NumberStyles.HexNumber)
                                                   + int.Parse(nextLine.Substring(1, 2), NumberStyles.HexNumber);
                                var nextLine2 = content[seq + 2].Trim();
                                if (LINEPERPACKET == 3 && nextLine2 != ":00000001FF" &&
                                    nextLine2.Substring(7, 2) == "00")
                                {
                                    checksum += int.Parse(nextLine2.Substring(nextLine2.Length - 2, 2),
                                                    NumberStyles.HexNumber)
                                                + int.Parse(nextLine2.Substring(3, 2), NumberStyles.HexNumber)
                                                + int.Parse(nextLine2.Substring(5, 2), NumberStyles.HexNumber);
                                    addresseByte += int.Parse(nextLine2.Substring(1, 3), NumberStyles.HexNumber);
                                    hexDataToSend = ":" + ("%0*X");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (WaitForHandshake(true) == HANDSHAKE_OK)
                {
                    _logger.Info("SUCCESS");
                    return true;
                }

                _logger.Warn(
                    "FAIL, IMG REFUSED BY TARGET (size exceeded? verify target MCU matches compiled target)");
                return false;
            }
            
            if (handshakeResponse == HANDSHAKE_FAIL_TIMEOUT)
            {
                _logger.Warn("FAIL: No response from Moteino programmer");
                return false;
            }

            _logger.Warn(
                "FAIL: No response from Moteino Target, is Target listening on same Freq/NetworkID & OTA enabled?");

            return false;
        }

        private void SerWriteLn(string msg)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(msg + "\n");
            _byteStream.Write(bytes, 0, bytes.Length);
            _logger.Trace(msg);
        }

        private const int HANDSHAKE_OK = 0;
        private const int HANDSHAKE_FAIL = 1;
        private const int HANDSHAKE_FAIL_TIMEOUT = 2;
        private const int HANDSHAKE_ERROR = 3;

        private int WaitForHandshake(bool isEOF = false)
        {
            var sw = Stopwatch.StartNew();
            while (true)
            {
                if (sw.ElapsedMilliseconds < 4000)
                {
                    if (isEOF)
                        SerWriteLn("FLX?EOF");
                    else
                    {
                        SerWriteLn("FLX?");
                    }

                    var rx = _byteStream.ReadLine().Trim().ToUpper();
                    if (!String.IsNullOrEmpty(rx))
                    {
                        if (rx == "FLX?OK")
                            return HANDSHAKE_OK;
                        if (rx == "FLX?NOK")
                            return HANDSHAKE_FAIL;
                        if (rx.Length > 7 && rx.StartsWith("FLX?NOK") || rx.StartsWith("FLX?ERR"))
                            return HANDSHAKE_ERROR;
                    }
                }
                else
                {
                    return HANDSHAKE_FAIL_TIMEOUT;
                }
            }
        }

        private bool WaitForTargetSet(int targetNode)
        {
            var sw = Stopwatch.StartNew();
            var to = $"TO:{targetNode}";

            SerWriteLn(to);
            while (true)
            {
                if (sw.ElapsedMilliseconds < 3000)
                {
                    var rx = _byteStream.ReadLine().Trim();
                    if (rx.Length > 0)
                        return rx == $"{to}:OK";
                }
                else
                {
                    return false;
                }
            }
        }

        private int WaitForSeq(string seq)
        {
            var sw = Stopwatch.StartNew();
            while (true)
            {
                if (sw.ElapsedMilliseconds < 3000)
                {
                    var rx = _byteStream.ReadLine().Trim();
                    if (rx.ToUpper().StartsWith("RFTX >") || rx.ToUpper().StartsWith("RFACK >"))
                    {
                        rx = String.Empty;
                        continue;
                    }

                    if (rx.Length > 0)
                    {
                        var pattern = new Regex("FLX:([0-9]*):OK");
                        var result = pattern.Match(rx);
                        if (result.Success)
                        {
                            if (result.Groups[1].Value == seq)
                                return 1;
                            return 2;
                        }

                        _logger.Warn($"Programmer reply '{rx}' does not match expected pattern: '{pattern}'");
                    }
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}