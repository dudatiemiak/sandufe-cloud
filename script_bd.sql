-- Bairros
CREATE TABLE IF NOT EXISTS `Bairros` (
  `IdBairro` int NOT NULL AUTO_INCREMENT,
  `NmBairro` varchar(100) NOT NULL,
  `IdCidade` int NOT NULL,
  PRIMARY KEY (`IdBairro`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4;

-- Cidades
CREATE TABLE IF NOT EXISTS `Cidades` (
  `IdCidade` int NOT NULL AUTO_INCREMENT,
  `NmCidade` varchar(50) NOT NULL,
  `IdEstado` int NOT NULL,
  PRIMARY KEY (`IdCidade`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4;

-- Clientes
CREATE TABLE IF NOT EXISTS `Clientes` (
  `IdCliente` int NOT NULL AUTO_INCREMENT,
  `NmCliente` varchar(100) NOT NULL,
  `NrCpf` varchar(14) NOT NULL,
  `NmEmail` varchar(100) NOT NULL,
  `IdLogradouro` int NOT NULL,
  PRIMARY KEY (`IdCliente`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4;

-- Departamentos
CREATE TABLE IF NOT EXISTS `Departamentos` (
  `IdDepartamento` int NOT NULL AUTO_INCREMENT,
  `NmDepartamento` varchar(50) NOT NULL,
  `DsDepartamento` varchar(250) NOT NULL,
  PRIMARY KEY (`IdDepartamento`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4;

-- Estados
CREATE TABLE IF NOT EXISTS `Estados` (
  `IdEstado` int NOT NULL AUTO_INCREMENT,
  `NmEstado` longtext NOT NULL,
  `IdPais` int NOT NULL,
  PRIMARY KEY (`IdEstado`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4;

-- Filiais
CREATE TABLE IF NOT EXISTS `Filiais` (
  `IdFilial` int NOT NULL AUTO_INCREMENT,
  `NmFilial` varchar(100) NOT NULL,
  `IdLogradouro` int NOT NULL,
  PRIMARY KEY (`IdFilial`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4;

-- FilialDepartamentos
CREATE TABLE IF NOT EXISTS `FilialDepartamentos` (
  `IdFilialDepartamento` int NOT NULL AUTO_INCREMENT,
  `IdFilial` int NOT NULL,
  `IdDepartamento` int NOT NULL,
  `DtEntrada` datetime(6) NOT NULL,
  `DtSaida` datetime(6) NULL,
  PRIMARY KEY (`IdFilialDepartamento`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4;

-- Funcionarios
CREATE TABLE IF NOT EXISTS `Funcionarios` (
  `IdFuncionario` int NOT NULL AUTO_INCREMENT,
  `NmFuncionario` varchar(100) NOT NULL,
  `NmCargo` varchar(50) NOT NULL,
  `NmEmailCorporativo` varchar(100) NOT NULL,
  `NmSenha` varchar(255) NOT NULL,
  `IdFilial` int NOT NULL,
  PRIMARY KEY (`IdFuncionario`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4;

-- Logradouros
CREATE TABLE IF NOT EXISTS `Logradouros` (
  `IdLogradouro` int NOT NULL AUTO_INCREMENT,
  `NmLogradouro` varchar(100) NOT NULL,
  `NrLogradouro` varchar(10) NOT NULL,
  `NmComplemento` varchar(100) NOT NULL,
  `IdBairro` int NOT NULL,
  PRIMARY KEY (`IdLogradouro`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4;

-- Manutencoes
CREATE TABLE IF NOT EXISTS `Manutencoes` (
  `IdManutencao` int NOT NULL AUTO_INCREMENT,
  `DtEntrada` datetime(6) NOT NULL,
  `DtSaida` datetime(6) NULL,
  `DsManutencao` varchar(300) NOT NULL,
  `IdMoto` int NOT NULL,
  PRIMARY KEY (`IdManutencao`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4;

-- Modelos
CREATE TABLE IF NOT EXISTS `Modelos` (
  `IdModelo` int NOT NULL AUTO_INCREMENT,
  `NmModelo` varchar(50) NOT NULL,
  PRIMARY KEY (`IdModelo`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4;

-- Motos
CREATE TABLE IF NOT EXISTS `Motos` (
  `IdMoto` int NOT NULL AUTO_INCREMENT,
  `NmPlaca` varchar(10) NOT NULL,
  `StMoto` varchar(30) NOT NULL,
  `KmRodado` double NOT NULL,
  `IdCliente` int NOT NULL,
  `IdModelo` int NOT NULL,
  `IdFilialDepartamento` int NOT NULL,
  PRIMARY KEY (`IdMoto`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4;

-- Paises
CREATE TABLE IF NOT EXISTS `Paises` (
  `IdPais` int NOT NULL AUTO_INCREMENT,
  `NmPais` varchar(50) NOT NULL,
  PRIMARY KEY (`IdPais`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4;


