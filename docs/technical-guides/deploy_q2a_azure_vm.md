# Deploying/Restoring Question2Answer on Azure VM

This guide outlines the process for setting up a testing environment on Azure by deploying or restoring Question2Answer on an Ubuntu VM. It includes detailed steps for creating a resource group, provisioning and configuring the VM, and setting up the Question2Answer application. Follow these instructions to ensure a smooth deployment and configuration for testing purposes.

## 1. Create a Resource Group

Create a resource group with the `az group create` command. An Azure resource group is a logical container into which Azure resources are deployed and managed.

Open Azure Cloud Shell, and type the following command:

```powershell
az group create --name rg-q2a-test-uksouth-001 --location uksouth
```
This example creates a resource group named rg-q2a-test-uksouth-001 in the uksouth region.

## 2. Create a Virtual Machine (VM)

Provision a VM using the `az vm create` command. This VM will serve as the host for your Question2Answer installation.

```powershell
az vm create --resource-group rg-q2a-test-uksouth-001 --name vm-q2a-test-uksouth-001 --image Ubuntu2204 --admin-username azureuser  --admin-password 'password'
```
In this example, a VM named vm-q2a-test-uksouth-001 is created with the Ubuntu 22.04 image, and an admin username and password are specified. Once the VM is created, the Azure CLI will display details for the VM. Be sure to take note of the publicIpAddress, as you'll need it later to connect to the VM.

## 3. Open Port 80 for HTTP Traffic

Open port 80 to allow HTTP traffic to your VM. This enables web access to your Question2Answer application.

```powershell
az vm open-port --port 80 --resource-group rg-q2a-test-uksouth-001 --name vm-q2a-test-uksouth-001
```
Port 80 is opened for HTTP traffic on the VM named vm-q2a-test-uksouth-001.

## 4. Open Port 3389 for RDP (Optional)

Open port 3389 for RDP access if you need to connect to the VM using a remote desktop protocol.

```powershell
az vm open-port --port 3389 --priority 1100 --resource-group rg-q2a-test-uksouth-001 --name vm-q2a-test-uksouth-001
```
This command allows remote desktop access to the VM with a priority of 1100.

## 5. Connect to the VM via SSH

Use SSH to connect to your VM and start configuring it.

Open your terminal or Git Bash on your machine and type the following command. Replace publicIpAddress with the actual public IP address of your VM.

```bash
ssh azureuser@publicIpAddress
```

## 6. Update and Install the LAMP Stack

Update the package lists and install the LAMP stack (Linux, Apache, MySQL, PHP) on your VM.

```bash
sudo apt update && sudo apt install lamp-server^
```
This installs the LAMP stack necessary for running Question2Answer.

## 7. Verify Installation

Check the versions of Apache, MySQL, and PHP to ensure they are installed correctly.

```bash
apache2 -v
mysql -V
php -v
```
These commands display the versions of Apache, MySQL, and PHP installed on your VM.

## 8. Secure MySQL Installation

Run the MySQL security script to improve the security of your MySQL installation.

```bash
sudo mysql_secure_installation
```
Follow the prompts to set the root password and configure security settings.

# 9. Configure MySQL Root User

Update the MySQL root user authentication method and set a new password.

```
sudo mysql

ALTER USER 'root'@'localhost' IDENTIFIED WITH mysql_native_password BY 'password';
exit
```
This command accesses MySQL, updates the root user authentication method, and sets a new password.

## 10. Create a PHP Info File (Optional)

To test further, you can create a simple PHP info page to view in your browser. Use the following command to create the PHP info page:

```bash
sudo sh -c 'echo "<?php phpinfo(); ?>" > /var/www/html/info.php'
```
After creating the page, open a browser and navigate to http://yourPublicIPAddress/info.php to view the PHP information.


## 11. Install and Configure Xfce and XRDP (Optional)

Install the Xfce desktop environment and XRDP for remote desktop access.

```bash
sudo DEBIAN_FRONTEND=noninteractive apt-get -y install xfce4
sudo apt install xfce4-session
sudo apt-get -y install xrdp
sudo systemctl enable xrdp
sudo adduser xrdp ssl-cert
echo xfce4-session >~/.xsession
sudo service xrdp restart
```
These commands will set up a remote desktop environment with Xfce and XRDP. After the installation is complete, open the remote desktop client and log in to the VM using the credentials configured during VM creation.

## 12. Install Google Chrome

To download and install Google Chrome on your VM, which you can then use to download the Question2Answer files, follow these commands:

```bash
wget https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb
sudo dpkg -i google-chrome-stable_current_amd64.deb
sudo apt --fix-broken install
```

## 13. Download and Set Up Question2Answer

If you want to work with the latest version of Question2Answer, download the newest release to your computer. If you're restoring from a backup, download your backup files instead.

- Install Unzip Utility: Install the unzip utility to extract the downloaded Question2Answer files.
  
```bash
sudo apt install unzip
```
- Unzip the Question2Answer Archive: Extract the Question2Answer zip file you downloaded.

```bash
unzip /home/azureuser/Downloads/question2answer-latest.zip
```

- Move Question2Answer to the Web Directory: Copy the extracted Question2Answer files to the default directory for Apache web files

```bash
sudo cp -r /home/azureuser/Downloads/question2answer-1.8.8/ /var/www/html
```

- Set Ownership and Permissions: Adjust the ownership and permissions of the Question2Answer files to ensure they are accessible by the web server.

```
sudo chown -R www-data:www-data /var/www/html/
sudo chmod -R 755 /var/www/html/
```

## 14. Configure MySQL for Question2Answer
Set up MySQL with a new user and database for Question2Answer.

```bash
mysql -u root -p
```
When asking for the password, set the password that we set for the root user in `step #9`

To list all existing MySQL users, you can use the following command:

```bash
SELECT User, Host FROM mysql.user;
```
```bash
CREATE USER 'q2aUser'@'localhost' IDENTIFIED BY 'password';
CREATE DATABASE question2answer;
SHOW DATABASES;
GRANT ALL PRIVILEGES ON question2answer.* TO 'q2aUser'@'localhost';
FLUSH PRIVILEGES;
```
These commands create a new MySQL user and database for Question2Answer and grant the necessary privileges to the new user on the new database.

## 15. Configure Question2Answer

Edits the Question2Answer configuration file (qa-config.php) to include database and other settings. 

```bash
sudo cp qa-config-example.php qa-config.php
sudo nano qa-config.php
```

`sudo cp qa-config-example.php qa-config.php`: Copies the example configuration file to a new configuration file.

`sudo nano qa-config.php`: Opens the configuration file in the Nano text editor.

Be sure to provide the database name, user, and password that we created in `step # 14` in this file.

## 16. Import Database (Optional)

If restoring from a backup, import the database using the following commands:

```bash
USE question2answer; # define database name

source /home/mradwan/Downloads/oldbackup.sql # define the database sql file location to be imported
```

After completing these steps, navigate to http://yourPublicIPAddress in a browser to access your Question2Answer installation. If you restored from a backup, the application should display the restored version.

## Troubleshooting  

If you encounter errors during installation on a Linux machine, try running the following commands to ensure your package lists and installed packages are up-to-date:
```bash
sudo apt upgrade
sudo apt update
```







