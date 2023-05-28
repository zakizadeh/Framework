Install using;

```bash
helm upgrade --install anz shared-mvc
```

Uninstall all charts

```bash
helm uninstall anz
```

## Create Images

```bash
docker build -t shared-mvc -f src\Htsc.Shared.Web.Mvc\Dockerfile .
docker build -t shared-migrator -f src\Htsc.Shared.Migrator\Dockerfile .
```
