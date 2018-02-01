FROM resin/amd64-fedora:25 AS build

# Using instructions from https://www.microsoft.com/net/core#linuxfedora
RUN rpm --import https://packages.microsoft.com/keys/microsoft.asc && \
    echo -e "[packages-microsoft-com-prod]\nname=packages-microsoft-com-prod \nbaseurl=https://packages.microsoft.com/yumrepos/microsoft-rhel7.3-prod\nenabled=1\ngpgcheck=1\ngpgkey=https://packages.microsoft.com/keys/microsoft.asc" \
    > /etc/yum.repos.d/dotnetdev.repo && \
    dnf install -y libunwind libicu dotnet-sdk-2.0.0

WORKDIR /usr/src/app
COPY . /usr/src/app
RUN dotnet publish -r linux-arm

FROM resin/armv7hf-debian

WORKDIR /usr/src/app

# Install dependencies listed at https://github.com/dotnet/core/blob/master/Documentation/prereqs.md
RUN apt-get update && \
    apt-get install --no-install-recommends -y \
    libunwind8 gettext liblttng-ust0 libcurl3  libuuid1 unzip && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

# Copy the standalone application
COPY --from=build /usr/src/app/bin/Debug/netcoreapp2.0/linux-arm/publish /usr/src/app