# ADR-001: Choosing Redis for Temporary Storage

## Context
The GoneSoon application requires a mechanism to temporarily store notes that expire after a predefined time. The storage solution should support automatic expiration and quick retrieval.

## Decision
We will use **Redis** as the temporary storage solution.

## Alternatives Considered
1. **SQL Server** – Requires manual cleanup of expired records, increasing complexity.
2. **MongoDB** – Supports TTL indexes but adds unnecessary overhead for a simple caching use case.
3. **Redis** – In-memory storage with built-in TTL support, making it ideal for this scenario.

## Justification
- **Automatic expiration**: Redis natively supports TTL (Time-To-Live) for keys, simplifying implementation.
- **Performance**: Being an in-memory database, Redis offers **fast read and write operations**.
- **Lightweight**: No need for complex queries; Redis operates efficiently as a temporary store.
- **Integration**: Redis is well-supported in .NET and can be easily managed via Docker.

## Consequences
✅ Simplifies auto-deletion of expired notes.  
✅ Improves response times due to in-memory storage.  
⚠ Requires memory management to avoid excessive usage.  
⚠ Adds an external dependency, requiring Redis to be available for proper application functionality.
