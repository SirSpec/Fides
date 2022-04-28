# Fides
A synchronization system to reliably transfer data between two separate data storage services.

## Use case and requirements
- Two separate service that store different data structures
- Host storage sends events such as: CREATE, UPDATE, DELETE
- Data being transfered in batches
- Data availability during transfer
- Data mapping
- Timestamp which indicates data change is not available
- Transfer only data that changed to decrease batch size during import
- Retry connection policy
- Circut breaking when the target service is unavailable
- System monitoring

## Ideas
- Scheduled jobs:
    - Transfer changed data records
    - Clean-up target service from non-existing data records
- Full reimport mechanism
- Events handling
