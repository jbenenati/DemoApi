#Run DemoApi first: C:\source\GitHub\DemoApi> dotnet run --project src/DemoApi

docker run --rm -t -v "${PWD}:/zap/wrk" ghcr.io/zaproxy/zaproxy:stable zap-api-scan.py -t http://host.docker.internal:5267/swagger/v1/swagger.json -f openapi -r zap_api_report.html -m 1