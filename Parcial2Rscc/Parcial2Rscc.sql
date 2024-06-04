-- Crear la base de datos
CREATE DATABASE Parcial2Rscc;
GO

-- Seleccionar la base de datos creada
USE [Parcial2Rscc];
GO

-- Crear el login y usuario
USE [master];
GO
CREATE LOGIN [usrparcial2] WITH PASSWORD=N'12345678',
    DEFAULT_DATABASE = [Parcial2Rscc],
    CHECK_EXPIRATION = OFF,
    CHECK_POLICY = ON;
GO
USE [Parcial2Rscc];
GO
CREATE USER [usrparcial2] FOR LOGIN [usrparcial2];
GO
ALTER ROLE [db_owner] ADD MEMBER [usrparcial2];
GO

-- Crear la tabla Serie
CREATE TABLE Serie (
    id INT IDENTITY(1,1) PRIMARY KEY,
    titulo VARCHAR(250),
    sinopsis VARCHAR(5000),
    director VARCHAR(100),
    episodios INT,
    fecha_estreno DATE,
    categoria VARCHAR(50)
);
GO

-- Agregar la columna estado a la tabla Serie
ALTER TABLE Serie ADD estado SMALLINT NOT NULL DEFAULT 1; -- -1: Eliminado, 0: Inactivo, 1: Activo
GO

CREATE PROCEDURE paSerieListar
    @parametro NVARCHAR(100)
AS
BEGIN
    SELECT id, titulo, sinopsis, director, episodios, fecha_estreno, categoria, estado
    FROM Serie
    WHERE (titulo LIKE '%' + @parametro + '%' OR sinopsis LIKE '%' + @parametro + '%')
    AND estado != -1
    ORDER BY titulo
END
-- Insertar datos de ejemplo en la tabla Serie
INSERT INTO Serie (titulo, sinopsis, director, episodios, fecha_estreno, categoria, estado)
VALUES
('Titanic', 'Romance', 'James Cameron', 1, '1997-12-19', 'Romance', 1),
('Avatar', 'Pandora', 'James Cameron', 1, '2009-12-18', 'Ciencia ficción', 1)

GO
