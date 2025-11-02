# Security Pipeline (v1)

## CI Gates
- **Build hygiene**: `/warnaserror` with `.editorconfig` raising security diagnostics to **error**.
- **SAST**: CodeQL (C#) on push/PR; Semgrep (OWASP Top 10) as fallback.
- **Secrets**: Gitleaks scans history & working tree; SARIF uploaded to code scanning.
- **Dependencies**: Dependabot weekly PRs; dependency-review blocks PRs adding high-severity vulns.

## Manual/Periodic
- **DAST**: OWASP ZAP API scan using Swagger/OpenAPI (report in `/docs/zap_api_report.html`).
- **Threat Modeling**: STRIDE snapshot in `/docs/ThreatModel.md`.

## Exceptions
- Any temporary allowlist requires: issue link, owner, expiry date, and compensating control.

## Baseline Metrics
- CodeQL: <fill in> (0 high)
- ZAP: <fill in> medium/high before → <fill in> after
- Secrets: 0 current, 0 historical
