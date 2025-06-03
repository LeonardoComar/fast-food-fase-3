# Definir a URL do endpoint de API para teste integrado
API_URL="https://www.google.com.br"

# Chamar o endpoint da API e verificar a resposta
response=$(curl --write-out "%{http_code}" --silent --output /dev/null "$API_URL")

# Se a resposta for 200, o teste integrado passou
if [ "$response" -eq 200 ]; then
  echo "Teste integrado sucesso: O endpoint da Api está respondendo corretamente."
  exit 0
else
  echo "Teste integrado falhou: O endpoint da Api não está respondendo corretamente."
  exit 1
fi