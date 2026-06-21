# TKL Computer

An e-commerce storefront for PC components. The project is split into two
decoupled parts:

- **Front-end** — ASP.NET Core (Razor Pages) serving the storefront UI and
  static assets. The homepage is `Pages/Index.cshtml`.
- **Back-end** — a Node.js + Express + SQL Server 2022 API in `js-controller/`
  (converted 1:1 from the old PHP backend).

## Structure

```
.
├── Program.cs                 # ASP.NET Core bootstrap (Razor Pages + MVC error infra)
├── may_tinh_sucvn.csproj
├── appsettings*.json
├── Controllers/               # HomeController (error handling only)
├── Models/
├── Pages/                     # Razor Pages — the storefront
│   ├── Index.cshtml           #   "/"  homepage
│   ├── cpu.cshtml … product.cshtml, contact.cshtml, terms.cshtml, 404.cshtml
│   └── Shared/                #   reusable partials
├── Views/                     # MVC views kept for the error page
├── wwwroot/                   # web root for all static assets
│   ├── css/  js/  images/  fonts/  video/
│   └── lib/                   #   vendored libs (bootstrap, jQuery) — untouched
├── js-controller/             # Node.js + Express + SQL Server backend (see its README)
├── database/                  # SQL schemas (MySQL original + sqlserver_schema.sql)
├── admin/                     # legacy PHP admin panel (NOT migrated — see notes)
└── src/Login.jsx              # stray React component (unused)
```

## Running

**Front-end (ASP.NET Core):**

```bash
dotnet restore
dotnet run         # serves the storefront; "/" -> Pages/Index.cshtml
```

**Back-end (API):** see [`js-controller/README.md`](js-controller/README.md).
Create the SQL Server schema from `database/sqlserver_schema.sql`, then `npm start`.

The front-end calls the API with root-relative paths (`/api/...`, `/login`,
`/checkout`, …). In deployment, host the Node service at the **same origin** as
the front-end (e.g. behind a reverse proxy). For local cross-origin dev, set
`FRONTEND_URL` in the API's `.env` and keep `credentials: 'include'` on fetches.

## Notes / follow-ups

- **`admin/` is not migrated.** It is legacy PHP that depended on the old
  `php/` includes (now removed). Port it to ASP.NET Core or a Node admin router
  if the admin panel is still needed.
- **Rotate the Gemini API key** that was hard-coded in the old
  `php/api_chatbot.php`; it is now read from `GEMINI_API_KEY` and should be
  considered compromised (it is in git history).
- The PHP → JS port of `database/database_schema.sql` (MySQL) is
  `database/sqlserver_schema.sql` (T-SQL).
