# Definir a URL do aplicativo
APP_URL="https://www.google.com.br"

# Verificar se o aplicativo está respondendo
response=$(curl --write-out "%{http_code}" --silent --output /dev/null "$APP_URL")

# Se a resposta for 200, o teste passou
if [ "$response" -eq 200 ]; then
  echo "Smoke test passou: Aplicação executando."
  exit 0
else
  echo "Smoke test falhou: Aplicação não está executando corretamente."
  exit 1
fi
