# Threat Model – DemoApi

## Dataflow
Client → (TLS) → ASP.NET Core API → SQL Server 2019 (container)
Trust boundaries: Internet ↔ API, API ↔ DB

## Assumptions
- HTTPS enforced (HSTS)
- Auth: (not implemented in v1 – treat endpoints as internal-only during mock)
- Secrets not committed; dev uses `dotnet user-secrets`

## STRIDE Summary
| Element           | Threat                      | Control                               | Gap                         | Action |
|------------------|-----------------------------|---------------------------------------|-----------------------------|--------|
| Query endpoint    | SQL Injection (T)           | Parameterized queries (Dapper)        | Legacy patterns possible    | CI regex test + CodeQL |
| API surface       | XSS via Swagger UI (T)      | CSP, nosniff, DENY frame              | Swagger CSP needs review    | Scope CSP to /swagger  |
| Transport         | MITM (S/T)                  | HTTPS + HSTS                          | None                        | Monitor cert expiry     |
| AuthZ             | Broken access control (E)   | N/A (demo only)                       | No authZ in v1              | Add policies/claims in v2 |
| Secrets           | Key leakage (I)             | Gitleaks + user-secrets               | N/A                         | Periodic scans          |
