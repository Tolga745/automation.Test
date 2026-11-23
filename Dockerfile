FROM mcr.microsoft.com/playwright/dotnet:v1.64.1-noble

WORKDIR /src

COPY automation.slnx ./
COPY automation.Tests ./automation.Tests
COPY automation.Report ./automation.Report

RUN dotnet restore automation.Tests/automation.Tests.csproj
RUN dotnet build automation.Tests/automation.Tests.csproj --no-restore
RUN dotnet restore automation.Report/automation.Report.csproj
RUN dotnet build automation.Report/automation.Report.csproj --no-restore

WORKDIR /src/test-results

COPY run-tests.sh /usr/local/bin/run-tests
RUN chmod +x /usr/local/bin/run-tests

ENTRYPOINT ["/usr/local/bin/run-tests"]
