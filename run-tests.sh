#!/bin/bash
set +e

mkdir -p /src/test-results
rm -f \
  /src/test-results/automation-results.trx \
  /src/test-results/test-report.html \
  /src/test-results/test-report.html.tmp

run_id=$(date -u +"%Y%m%dT%H%M%SZ")
trx_path="/src/test-results/automation-results-${run_id}.trx"
html_path="/src/test-results/test-report.html"

dotnet test /src/automation.Tests/automation.Tests.csproj \
  --no-build \
  --no-restore \
  --logger "trx;LogFileName=$(basename "$trx_path")" \
  --results-directory /src/test-results \
  --logger "console;verbosity=detailed" \
  "$@"

test_status=$?

dotnet run --project /src/automation.Report/automation.Report.csproj -- \
  "$trx_path" \
  "$html_path"

report_status=$?

if [ "$test_status" -ne 0 ]; then
  exit "$test_status"
fi

exit "$report_status"
