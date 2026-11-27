-- ============================================
-- SCRIPT 2: Datos de prueba adicionales
-- (ejecutar una sola vez, después del Script 1)
-- ============================================
USE dbadaptacionuniversitaria;

-- Usuarios nuevos: 4 estudiantes (uno inactivo) y 2 consejeros
INSERT INTO usuarios (rol_id, nombres, apellidos, email, password_hash, activo) VALUES
(1, 'Camila', 'Mamani Quispe', 'camila.mamani@ucb.test', 'HASH_DE_PRUEBA', 1),
(1, 'Diego', 'Rojas Vargas', 'diego.rojas@ucb.test', 'HASH_DE_PRUEBA', 1),
(1, 'Valeria', 'Flores Condori', 'valeria.flores@ucb.test', 'HASH_DE_PRUEBA', 1),
(1, 'Joaquín', 'Vega Choque', 'joaquin.vega@ucb.test', 'HASH_DE_PRUEBA', 0),
(2, 'Laura', 'Gutiérrez Salazar', 'laura.gutierrez@ucb.test', 'HASH_DE_PRUEBA', 1),
(2, 'Carlos', 'Mendoza Arce', 'carlos.mendoza@ucb.test', 'HASH_DE_PRUEBA', 1);

-- Variables con los IDs (se buscan por correo)
SET @est = (SELECT id FROM usuarios WHERE email = 'estudiante@ucb.test');
SET @camila = (SELECT id FROM usuarios WHERE email = 'camila.mamani@ucb.test');
SET @diego = (SELECT id FROM usuarios WHERE email = 'diego.rojas@ucb.test');
SET @valeria = (SELECT id FROM usuarios WHERE email = 'valeria.flores@ucb.test');
SET @laura = (SELECT id FROM usuarios WHERE email = 'laura.gutierrez@ucb.test');
SET @carlos = (SELECT id FROM usuarios WHERE email = 'carlos.mendoza@ucb.test');

-- Bitácoras (estado_animo de 1 a 5; sintomas y nota son opcionales)
INSERT INTO bitacoras (estudiante_id, fecha, estado_animo, sintomas, nota) VALUES
(@est, '2026-10-14', 4, NULL, 'Buen día de clases'),
(@est, '2026-10-15', 3, 'Cansancio', 'Entrega de proyecto'),
(@camila, '2026-10-12', 4, NULL, 'Primer día de la semana tranquilo'),
(@camila, '2026-10-13', 3, 'Dolor de cabeza leve', 'Mucha tarea acumulada'),
(@camila, '2026-10-14', 2, 'Insomnio y ansiedad', 'Examen parcial mañana'),
(@camila, '2026-10-15', 4, 'Cansancio', 'Me fue bien en el examen'),
(@camila, '2026-10-16', 5, NULL, 'Me siento motivada'),
(@diego, '2026-10-13', 3, 'Falta de concentración', NULL),
(@diego, '2026-10-14', 2, 'Cansancio y desánimo', 'Me cuesta adaptarme a los horarios'),
(@diego, '2026-10-15', 3, NULL, 'Hablé con un compañero y me ayudó'),
(@valeria, '2026-10-14', 1, 'Tristeza e insomnio', 'Extraño mi casa'),
(@valeria, '2026-10-15', 2, 'Falta de apetito', NULL),
(@valeria, '2026-10-16', 3, 'Cansancio', 'Un poco mejor hoy');

-- Citas en distintos estados (solo Diego tiene una pendiente, además del estudiante de prueba)
INSERT INTO citas (estudiante_id, consejero_id, fecha_hora, motivo, estado, motivo_rechazo,
creado_en, respondido_en) VALUES
(@camila, @laura, '2026-10-21 10:00:00', 'Ansiedad antes de los exámenes',
'Aprobada', NULL, '2026-10-16 18:00:00', '2026-10-17 09:30:00'),
(@camila, @carlos, '2026-10-18 14:00:00', 'Consulta sobre manejo del tiempo',
'Cancelada', NULL, '2026-10-15 12:00:00', NULL),
(@diego, @laura, '2026-10-22 16:00:00', 'Dificultad para adaptarme a la universidad',
'Pendiente', NULL, '2026-10-19 20:15:00', NULL),
(@valeria, @carlos, '2026-10-19 11:00:00', 'Me siento sola y extraño mi casa',
'Aprobada', NULL, '2026-10-16 17:40:00', '2026-10-17 10:05:00'),
(@valeria, @carlos, '2026-10-23 09:00:00', 'Seguimiento de mi primera cita',
'Rechazada', 'Horario no disponible', '2026-10-19 21:00:00', '2026-10-20 08:30:00');
