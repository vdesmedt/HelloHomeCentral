using System.Globalization;

namespace HelloHome.Central.Common.Mqtt.Topic;

public enum Scope { Core, Node }
public enum Kind  { Command, Report }
public enum Command { Restart, Config, Ping, Stat }
public enum Report { Started, Environment, Pulses, Pong, Performance }

public sealed class HhTopic : IEquatable<HhTopic>
{
    public Scope TargetScope { get; }
    public int? NodeId { get; }
    public Kind PathKind { get; }
    public string Name { get; } // last token

    // Convenience typed getters (null when not that kind)
    public Command? Command => PathKind == Kind.Command && Enum.TryParse<Command>(Name, true, out var c) ? c : null;
    public Report?  Report  => PathKind == Kind.Report  && Enum.TryParse<Report >(Name, true, out var r) ? r : null;

    public bool IsCore => TargetScope == Scope.Core;
    public bool IsNode => TargetScope == Scope.Node;

    private HhTopic(Scope scope, int? nodeId, Kind kind, string name)
    {
        if (scope == Scope.Node && nodeId is null)
            throw new ArgumentException("NodeId is required for node scope.");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));

        TargetScope = scope;
        NodeId = nodeId;
        PathKind = kind;
        Name = name;
    }

    public override string ToString()
    {
        string root = TargetScope == Scope.Core ? "core" : $"node/{NodeId.Value}";
        string kind  = PathKind switch
        {
            Kind.Command => "command",
            Kind.Report  => "report",
            _ => throw new InvalidOperationException("Unknown path kind.")
        };
        return $"{root}/{kind}/{Name}";
    }

    #region Parsing
    public static HhTopic Parse(string path)
    {
        if (path is null) throw new ArgumentNullException(nameof(path));

        var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        // Valid shapes:
        // core/{command|report|event}/{name}
        // node/{id}/{command|report|event}/{name}
        if (parts.Length == 3 && parts[0].Equals("core", StringComparison.OrdinalIgnoreCase))
        {
            return new HhTopic(
                Scope.Core,
                null,
                ParseKind(parts[1]),
                parts[2]
            );
        }
        if (parts.Length == 4 && parts[0].Equals("node", StringComparison.OrdinalIgnoreCase))
        {
            if (!int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var id) || id < 0)
                throw new FormatException("Invalid node id.");

            return new HhTopic(
                Scope.Node,
                id,
                ParseKind(parts[2]),
                parts[3]
            );
        }

        throw new FormatException("Invalid path format.");
    }

    public static bool TryParse(string path, out HhTopic? result)
    {
        try { result = Parse(path); return true; }
        catch { result = null; return false; }
    }

    private static Kind ParseKind(string token) => token.ToLowerInvariant() switch
    {
        "command" => Kind.Command,
        "report"  => Kind.Report,
        _ => throw new FormatException("Unknown kind token. Expected 'command', 'report' or 'event'.")
    };
    #endregion

    #region Fluent builders
    public static CommandBuilder ForCommand(Command command) =>
        new CommandBuilder(EnumToToken(command).ToLower());

    public static ReportBuilder ForReport(Report report) =>
        new ReportBuilder(EnumToToken(report));
    
    public readonly struct CommandBuilder
    {
        private readonly string _name;
        internal CommandBuilder(string name) => _name = name;

        public HhTopic ToCore()       => new HhTopic(Scope.Core, null, Kind.Command, _name);
        public HhTopic ToNode(int id) => new HhTopic(Scope.Node, id, Kind.Command, _name);
    }

    public readonly struct ReportBuilder
    {
        private readonly string _name;
        internal ReportBuilder(string name) => _name = name;

        public HhTopic ToCore()       => new HhTopic(Scope.Core, null, Kind.Report, _name);
        public HhTopic ToNode(int id) => new HhTopic(Scope.Node, id, Kind.Report, _name);
    }
    #endregion

    #region Equality
    public bool Equals(HhTopic? other) =>
        other is not null &&
        TargetScope == other.TargetScope &&
        NodeId == other.NodeId &&
        PathKind == other.PathKind &&
        string.Equals(Name, other.Name, StringComparison.Ordinal);

    public override bool Equals(object? obj) => obj is HhTopic mp && Equals(mp);

    public override int GetHashCode() =>
        HashCode.Combine(TargetScope, NodeId, PathKind, Name);
    #endregion

    #region Helpers
    private static string EnumToToken<T>(T value) where T : struct, Enum
        => value.ToString(); // default ToString keeps PascalCase; adjust here if you need kebab/snake.
    
    #endregion
}