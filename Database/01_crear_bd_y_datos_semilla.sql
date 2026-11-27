-- ============================================
-- SCRIPT 1: Creación de la base de datos, tablas y datos semilla
-- (un usuario por rol)
-- ============================================
CREATE DATABASE IF NOT EXISTS dbadaptacionuniversitaria
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;
USE dbadaptacionuniversitaria;

CREATE TABLE roles (
  id INT NOT NULL AUTO_INCREMENT,
  nombre VARCHAR(30) NOT NULL,
  PRIMARY KEY (id),
  UNIQUE KEY uk_roles_nombre (nombre)
);

CREATE TABLE usuarios (
  id INT NOT NULL AUTO_INCREMENT,
  rol_id INT NOT NULL,
  nombres VARCHAR(100) NOT NULL,
  apellidos VARCHAR(100) NOT NULL,
  email VARCHAR(255) NOT NULL,
  password_hash VARCHAR(255) NOT NULL,
  activo TINYINT(1) NOT NULL DEFAULT 1,
  creado_en DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  UNIQUE KEY uk_usuarios_email (email),
  CONSTRAINT fk_usuarios_rol FOREIGN KEY (rol_id) REFERENCES roles (id)
);

CREATE TABLE bitacoras (
  id INT NOT NULL AUTO_INCREMENT,
  estudiante_id INT NOT NULL,
  fecha DATE NOT NULL,
  estado_animo TINYINT NOT NULL,
  sintomas VARCHAR(255) NULL,
  nota TEXT NULL,
  creado_en DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  UNIQUE KEY uk_bitacoras_estudiante_fecha (estudiante_id, fecha),
  CONSTRAINT fk_bitacoras_estudiante FOREIGN KEY (estudiante_id) REFERENCES usuarios (id),
  CONSTRAINT ck_bitacoras_animo CHECK (estado_animo BETWEEN 1 AND 5)
);

CREATE TABLE citas (
  id INT NOT NULL AUTO_INCREMENT,
  estudiante_id INT NOT NULL,
  consejero_id INT NOT NULL,
  fecha_hora DATETIME NOT NULL,
  motivo VARCHAR(500) NOT NULL,
  estado VARCHAR(20) NOT NULL DEFAULT 'Pendiente',
  motivo_rechazo VARCHAR(500) NULL,
  creado_en DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  respondido_en DATETIME NULL,
  PRIMARY KEY (id),
  CONSTRAINT fk_citas_estudiante FOREIGN KEY (estudiante_id) REFERENCES usuarios (id),
  CONSTRAINT fk_citas_consejero FOREIGN KEY (consejero_id) REFERENCES usuarios (id),
  CONSTRAINT ck_citas_estado CHECK (estado IN ('Pendiente', 'Aprobada', 'Rechazada', 'Cancelada'))
);

-- Datos semilla: un usuario por rol (datos ficticios)
INSERT INTO roles (nombre) VALUES ('Estudiante'), ('Consejero'), ('Administrador');

INSERT INTO usuarios (rol_id, nombres, apellidos, email, password_hash) VALUES
(1, 'Estudiante', 'De Prueba', 'estudiante@ucb.test', 'HASH_DE_PRUEBA_ESTUDIANTE'),
(2, 'Consejero', 'De Prueba', 'consejero@ucb.test', 'HASH_DE_PRUEBA_CONSEJERO'),
(3, 'Administrador', 'De Prueba', 'admin@ucb.test', 'HASH_DE_PRUEBA_ADMIN');

INSERT INTO bitacoras (estudiante_id, fecha, estado_animo, sintomas, nota) VALUES
(1, '2026-10-16', 3, 'Cansancio y falta de concentración', 'Semana de evaluaciones');

INSERT INTO citas (estudiante_id, consejero_id, fecha_hora, motivo) VALUES
(1, 2, '2026-10-20 15:00:00', 'Orientación para organizar mi tiempo de estudio');
