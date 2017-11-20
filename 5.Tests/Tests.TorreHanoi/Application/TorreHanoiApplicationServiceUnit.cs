﻿using System;
using System.Collections.Generic;
using System.Net;
using Application.TorreHanoi.Implementation;
using Application.TorreHanoi.Interface;
using Domain.TorreHanoi.Interface.Service;
using Infrastructure.TorreHanoi.ImagemHelper;
using Infrastructure.TorreHanoi.Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;
using System.Linq;
using Domain.TorreHanoi;

namespace Tests.TorreHanoi.Application
{
    [TestClass]
    public class TorreHanoiApplicationServiceUnit
    {
        private const string CategoriaTeste = "Application/Service/TorreHanoi";

        private ITorreHanoiApplicationService _service;
        private Mock<ILogger> _mockLogger;

        [TestInitialize]
        public void SetUp()
        {

            _mockLogger = new Mock<ILogger>();
            _mockLogger.Setup(s => s.Logar(It.IsAny<string>(), It.IsAny<TipoLog>()));

            var mockDesignerService = new Mock<IDesignerService>();

            var mockTorreHanoiDomainService = new Mock<ITorreHanoiDomainService>();
            mockTorreHanoiDomainService.Setup(s => s.Criar(It.IsAny<int>())).Returns(Guid.NewGuid);
            mockTorreHanoiDomainService.Setup(s => s.ObterPor(It.IsAny<Guid>())).Returns(() => new global::Domain.TorreHanoi.TorreHanoi(3, _mockLogger.Object));
            mockTorreHanoiDomainService.Setup(s => s.ObterTodos()).Returns(() => new List<global::Domain.TorreHanoi.TorreHanoi> { new global::Domain.TorreHanoi.TorreHanoi(3, _mockLogger.Object) });

            _service = new TorreHanoiApplicationService(mockTorreHanoiDomainService.Object, _mockLogger.Object, mockDesignerService.Object);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void AdicionarNovoProcesso_Deve_Retornar_Sucesso()
        {
            var response = _service.AdicionarNovoPorcesso(3);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Accepted);
            Assert.AreNotEqual(response.IdProcesso, new Guid());
            Assert.IsTrue(response.IsValid);
            Assert.IsTrue(response.MensagensDeErro.Count == 0);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void ObterProcessoPor_Deverar_Retornar_Sucesso()
        {
            var response = _service.ObterProcessoPor(Guid.NewGuid().ToString());

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(response.Processo);
            Assert.IsTrue(response.IsValid);
            Assert.IsTrue(response.MensagensDeErro.Count == 0);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void ObterTodosProcessos_Deverar_Retornar_Sucesso()
        {
            var response = _service.ObterTodosProcessos();

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(response.Processos);
            Assert.IsTrue(response.Processos.Count > 0);
            Assert.IsTrue(response.IsValid);
            Assert.IsTrue(response.MensagensDeErro.Count == 0);
        }


        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void ObterImagemProcessoPor_Deve_Retornar_Imagem()
        {
           
            var addproces = _service.AdicionarNovoPorcesso(3);
            var torre = new global::Domain.TorreHanoi.TorreHanoi(3, _mockLogger.Object);
            torre.Processar();

            Assert.AreEqual(torre.Status, TipoStatus.FinalizadoSucesso);

            var result =  _service.ObterImagemProcessoPor(addproces.IdProcesso.ToString());

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.MensagensDeErro.Count == 0);

        }
    }
}
