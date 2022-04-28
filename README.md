# Fides
A synchronization system to reliably transfer data between two separate data storage services.

## Use case and requirements
- Two separate service that store different data structures
- Both services provide only an API to QUERY, CREATE, UPDATE, DELETE data
- Host storage sends events such as: CREATE, UPDATE, DELETE
- Data being transfered in batches
- Data availability during transfer
- Data Consistency
- Data mapping
- Timestamp which indicates data change is not available
- Transfer only data that changed to decrease batch size during import
- Handle network timeouts
- System monitoring

## Ideas
- Scheduled jobs
    - Transfer changed data records
    - Clean-up target service from non-existing data records
- Full reimport mechanism
- Events handling
    - Data sync when event occurs
- Each connection is protected by retry policy
- Circut breaking when the target service is unavailable
- Queue-Based Load Leveling
- Telemetry

## Tech stack
- [.NET](https://dotnet.microsoft.com/en-us/download)
- [Docker](https://www.docker.com)
- [MS SQL](https://hub.docker.com/_/microsoft-mssql-server)
- [Polly](https://github.com/App-vNext/Polly)
- [NLog](https://nlog-project.org)
- [RabbitMQ](https://www.rabbitmq.com)
- [Kibana](https://www.elastic.co/kibana/)

## References
- [Data Consistency Primer](https://docs.microsoft.com/en-us/previous-versions/msp-n-p/dn589800(v=pandp.10))
- [Queue-Based Load Leveling](https://docs.microsoft.com/en-us/azure/architecture/patterns/queue-based-load-leveling)
- [Retry](https://docs.microsoft.com/en-us/azure/architecture/patterns/retry)
- [Circuit Breaker](https://docs.microsoft.com/en-us/azure/architecture/patterns/circuit-breaker)
- [Rate Limiting](https://docs.microsoft.com/en-us/azure/architecture/patterns/rate-limiting-pattern)
