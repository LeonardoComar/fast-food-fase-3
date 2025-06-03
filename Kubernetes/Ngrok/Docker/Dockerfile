# Usar a imagem base do Debian
FROM debian:bullseye-slim

# Instalar dependências necessárias para adicionar o repositório e o ngrok
RUN apt-get update && \
    apt-get install -y curl gnupg lsb-release sudo && \
    # Adicionar a chave e repositório do Ngrok
    curl -sSL https://ngrok-agent.s3.amazonaws.com/ngrok.asc \
    | sudo tee /etc/apt/trusted.gpg.d/ngrok.asc >/dev/null && \
    echo "deb https://ngrok-agent.s3.amazonaws.com buster main" \
    | sudo tee /etc/apt/sources.list.d/ngrok.list && \
    # Atualizar e instalar o Ngrok
    apt-get update && \
    apt-get install -y ngrok && \
    # Limpar o cache do apt para reduzir o tamanho da imagem
    apt-get clean

# Definir o comando de execução para o contêiner
ENTRYPOINT ["/usr/bin/ngrok"]
