### 📝 Documentación para despliegue de .NET Core y Angular con AWS CloudFormation

Esta documentación te guiará para desplegar una aplicación .NET Core y Angular en AWS, utilizando una plantilla de **CloudFormation** para automatizar la creación de la infraestructura. La plantilla que te proporciono crea una instancia de **EC2**, instala y configura los servicios necesarios, y clona tu código desde un repositorio.

### 📄 Plantilla de CloudFormation (YAML)

El siguiente es el código YAML de la plantilla. Puedes copiarlo y guardarlo en un archivo con extensión `.yaml` para usarlo en AWS.

```yaml
AWSTemplateFormatVersion: '2010-09-09'
Description: Despliegue de una aplicación .NET Core y Angular en una instancia EC2

Parameters:
  InstanceType:
    Description: Tipo de instancia EC2 para el servidor de la aplicación.
    Type: String
    Default: t2.medium
  
  KeyPairName:
    Description: Nombre de la clave EC2 (la necesitarás para conectarte por SSH).
    Type: AWS::EC2::KeyPair::KeyName
  
  VpcId:
    Description: ID de la VPC donde se desplegará la instancia.
    Type: AWS::EC2::VPC::Id
  
  SubnetId:
    Description: ID de la subred donde se lanzará la instancia.
    Type: AWS::EC2::Subnet::Id

Resources:
  EC2Instance:
    Type: AWS::EC2::Instance
    Properties:
      InstanceType: !Ref InstanceType
      KeyName: !Ref KeyPairName
      ImageId: ami-0abcdef1234567890 # Reemplaza con una AMI de Amazon Linux 2 (asegúrate de que sea la correcta para tu región).
      Tags:
        - Key: Name
          Value: !Sub '${AWS::StackName}-Instance'
      UserData:
        Fn::Base64: |
          #!/bin/bash
          # Instalar dependencias esenciales
          yum update -y
          yum install git unzip -y

          # Instalar .NET Core SDK 6.0
          wget https://packages.microsoft.com/config/centos/7/packages-microsoft-prod.rpm -O packages-microsoft-prod.rpm
          rpm -ivh packages-microsoft-prod.rpm
          yum install dotnet-sdk-6.0 -y

          # Instalar Node.js 16 y Angular CLI
          curl -sL https://rpm.nodesource.com/setup_16.x | bash -
          yum install -y nodejs
          npm install -g @angular/cli

          # Clonar el repositorio de la aplicación
          git clone https://github.com/tu-usuario/tu-repositorio.git /var/www/app
          
          # Desplegar el front-end (Angular)
          cd /var/www/app/angular-app
          npm install
          ng build --prod

          # Desplegar el back-end (.NET Core)
          cd /var/www/app/dotnet-api
          dotnet restore
          dotnet publish -o /var/www/publish

          # Instalar y configurar Nginx
          amazon-linux-extras install nginx1.12 -y
          systemctl start nginx
          systemctl enable nginx

          # Crear el archivo de configuración de Nginx
          cat > /etc/nginx/nginx.conf <<EOF
          user nginx;
          worker_processes auto;
          events {
              worker_connections 1024;
          }
          http {
              include       /etc/nginx/mime.types;
              default_type  application/octet-stream;
              sendfile        on;
              keepalive_timeout  65;
              server {
                  listen 80;
                  location / {
                      root   /var/www/app/angular-app/dist/angular-app;
                      index  index.html;
                      try_files $uri $uri/ /index.html;
                  }
                  location /api {
                      proxy_pass http://localhost:5000;
                      proxy_set_header Host $host;
                      proxy_set_header X-Real-IP $remote_addr;
                      proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
                      proxy_set_header X-Forwarded-Proto $scheme;
                  }
              }
          }
          EOF

          # Reiniciar Nginx para aplicar los cambios
          systemctl restart nginx

          # Configurar la aplicación .NET Core como un servicio de systemd
          cat > /etc/systemd/system/kestrel-app.service <<EOF
          [Unit]
          Description=Aplicación .NET Core
          
          [Service]
          WorkingDirectory=/var/www/publish
          ExecStart=/usr/bin/dotnet /var/www/publish/tu-aplicacion.dll # Reemplaza con el nombre de tu DLL principal.
          Restart=always
          RestartSec=10
          KillSignal=SIGINT
          SyslogIdentifier=dotnet-app
          User=nginx
          Environment=ASPNETCORE_ENVIRONMENT=Production
          
          [Install]
          WantedBy=multi-user.target
          EOF
          
          # Habilitar y arrancar el servicio de .NET
          systemctl enable kestrel-app.service
          systemctl start kestrel-app.service
```

-----

### 🚀 Pasos para el Despliegue en AWS

1.  **Guarda la plantilla**: Copia el código anterior y guárdalo en un archivo de texto plano llamado `template.yaml`.
2.  **Sube la plantilla a CloudFormation**:
      * Navega a la consola de AWS y ve al servicio **CloudFormation**.
      * Haz clic en **"Crear pila"**.
      * Selecciona **"Cargar un archivo de plantilla"** y sube el archivo `template.yaml` que acabas de crear.
      * Haz clic en **"Siguiente"**.
3.  **Configura los parámetros**:
      * Asigna un **nombre a la pila**.
      * Proporciona los valores requeridos para `InstanceType`, `KeyPairName`, `VpcId` y `SubnetId`.
4.  **Revisa y lanza la pila**:
      * Revisa los detalles de la pila y, si todo es correcto, haz clic en **"Crear pila"**.

CloudFormation se encargará de aprovisionar y configurar todos los recursos por ti. Puedes seguir el progreso en la pestaña **"Eventos"** de tu pila.

-----

Si tienes alguna duda sobre algún paso específico, no dudes en preguntar.