using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using DTOs;
using Domain;
using Domain.Enums;
using IDataAcces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;

namespace Controllers_Tests
{
    [TestClass]
    public class AsignacionRecursoTareaControllersTests
    {
        private Mock<IAsignacionRecursoTareaService> _serviceMock;
        private AsignacionRecursoTareaControllers _controller;
        private readonly RecursoDTO _recursoDto = new RecursoDTO { Id = 42 };
        private readonly TareaDTO   _tareaDto   = new TareaDTO   { Id = 99 };

        [TestInitialize]
        public void SetUp()
        {
            _serviceMock = new Mock<IAsignacionRecursoTareaService>(MockBehavior.Strict);
            _controller  = new AsignacionRecursoTareaControllers(_serviceMock.Object);
        }

        [TestMethod]
        public void GetAll_ShouldReturnServiceList()
        {
            List<AsignacionRecursoTareaDTO> esperado = new List<AsignacionRecursoTareaDTO>
            {
                new AsignacionRecursoTareaDTO { Id = 1, Recurso = _recursoDto, Tarea = _tareaDto, Cantidad = 3 }
            };
            _serviceMock
                .Setup(s => s.GetAll())
                .Returns(esperado);

            List<AsignacionRecursoTareaDTO> actual = _controller.GetAll();

            Assert.AreSame(esperado, actual);
            _serviceMock.Verify(s => s.GetAll(), Times.Once);
        }

        [TestMethod]
        public void GetById_ShouldReturnServiceValue()
        {
            AsignacionRecursoTareaDTO dto = new AsignacionRecursoTareaDTO { Id = 7, Recurso = _recursoDto, Tarea = _tareaDto, Cantidad = 2 };
            _serviceMock
                .Setup(s => s.GetById(7))
                .Returns(dto);
            AsignacionRecursoTareaDTO resultado = _controller.GetById(7);
            Assert.AreEqual(7, resultado.Id);
            Assert.AreEqual(2, resultado.Cantidad);
            _serviceMock.Verify(s => s.GetById(7), Times.Once);
        }

        [TestMethod]
        public void CrearAsignacionRecursoTarea_ShouldCallServiceAndReturn()
        {
            AsignacionRecursoTareaDTO inputDto = new AsignacionRecursoTareaDTO { Recurso = _recursoDto, Tarea = _tareaDto, Cantidad = 5 };
            AsignacionRecursoTareaDTO creado  = new AsignacionRecursoTareaDTO { Id = 13, Recurso = _recursoDto, Tarea = _tareaDto, Cantidad = 5 };
            _serviceMock
                .Setup(s => s.CrearAsignacionRecursoTarea(inputDto))
                .Returns(creado);

            AsignacionRecursoTareaDTO resultado = _controller.CrearAsignacionRecursoTarea(inputDto);

            Assert.AreEqual(13, resultado.Id);
            Assert.AreEqual(5, resultado.Cantidad);
            _serviceMock.Verify(s => s.CrearAsignacionRecursoTarea(inputDto), Times.Once);
        }

        [TestMethod]
        public void EliminarRecursoDeTarea_ShouldCallService()
        {
            int tareaId   = 7, recursoId = 21;
            _serviceMock
                .Setup(s => s.EliminarRecursoDeTarea(tareaId, recursoId));

            _controller.EliminarRecursoDeTarea(tareaId, recursoId);

            _serviceMock.Verify(s => s.EliminarRecursoDeTarea(tareaId, recursoId), Times.Once);
        }

        [TestMethod]
        public void ModificarAsignacion_ShouldCallService()
        {
            AsignacionRecursoTareaDTO dto = new AsignacionRecursoTareaDTO { Id = 5, Cantidad = 9 };
            _serviceMock
                .Setup(s => s.ModificarAsignacion(dto));

            _controller.ModificarAsignacion(dto);

            _serviceMock.Verify(s => s.ModificarAsignacion(dto), Times.Once);
        }

        [TestMethod]
        public void RecursosDeLaTarea_ShouldReturnServiceList()
        {
            List<RecursoDTO> recursos = new List<RecursoDTO>
            {
                new RecursoDTO { Id = 2 },
                new RecursoDTO { Id = 3 },
            };
            _serviceMock
                .Setup(s => s.RecursosDeLaTarea(99))
                .Returns(recursos);

            var resultado = _controller.RecursosDeLaTarea(99);
            CollectionAssert.AreEqual(recursos, resultado);
            _serviceMock.Verify(s => s.RecursosDeLaTarea(99), Times.Once);
        }

        [TestMethod]
        public void EliminarRecursosDeTarea_ShouldCallService()
        {
            int tareaId = 5;
            _serviceMock.Setup(s => s.EliminarRecursosDeTarea(tareaId));
            _controller.EliminarRecursosDeTarea(tareaId);

            _serviceMock.Verify(s => s.EliminarRecursosDeTarea(tareaId), Times.Once);
        }

        [TestMethod]
        public void ActualizarEstadoDeTareasConRecurso_ShouldCallService()
        {
            int recursoId = 7;
            _serviceMock.Setup(s => s.ActualizarEstadoDeTareasConRecurso(recursoId));

            _controller.ActualizarEstadoDeTareasConRecurso(recursoId);
            _serviceMock.Verify(s => s.ActualizarEstadoDeTareasConRecurso(recursoId), Times.Once);
        }

        [TestMethod]
        public void RecursoEsExclusivo_ShouldReturnServiceValue()
        {
            _serviceMock.Setup(s => s.RecursoEsExclusivo(3)).Returns(true);

            bool result = _controller.RecursoEsExclusivo(3);
            Assert.IsTrue(result);
            _serviceMock.Verify(s => s.RecursoEsExclusivo(3), Times.Once);
        }

        [TestMethod]
        public void VerificarRecursosDeTareaDisponibles_ShouldReturnServiceValue()
        {
            _serviceMock.Setup(s => s.VerificarRecursosDeTareaDisponibles(10)).Returns(false);

            bool result = _controller.VerificarRecursosDeTareaDisponibles(10);
            Assert.IsFalse(result);
            _serviceMock.Verify(s => s.VerificarRecursosDeTareaDisponibles(10), Times.Once);
        }

        [TestMethod]
        public void GetAsignacionesDeTarea_ShouldReturnServiceList()
        {
            List<AsignacionRecursoTareaDTO> asigns = new List<AsignacionRecursoTareaDTO>
            {
                new AsignacionRecursoTareaDTO { Id = 1, Cantidad = 2, Recurso = _recursoDto, Tarea = _tareaDto }
            };
            _serviceMock
                .Setup(s => s.GetAsignacionesDeTarea(_tareaDto.Id))
                .Returns(asigns);

            List<AsignacionRecursoTareaDTO> resultado = _controller.GetAsignacionesDeTarea(_tareaDto.Id);

            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual(asigns, resultado);
            _serviceMock.Verify(s => s.GetAsignacionesDeTarea(_tareaDto.Id), Times.Once);
        }
    }
}
