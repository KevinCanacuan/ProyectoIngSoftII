using prjLegados.AzureStorage.Blobs;
using prjLegados.Data.Structure;
using prjLegados.AzureStorage.Helper;
using prjLegados.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prjLegados.Data.AzureEntities;
using prjLegados.AzureStorage.Search;
using System.Configuration;

namespace prjLegados.Controllers
{
    public class ContainerController : Controller
    {
        BlobStorage blobStorage = new BlobStorage();
        List<string> lstArchivos = new List<string>();
        ClsContainer guardar = new ClsContainer();
        User usrUser = new User();

        // GET: Container
        public ActionResult Container()
        {
            //TODO: 

            ClsContainer cntContenedores = new ClsContainer();

            cntContenedores.lstContainers = blobStorage.fntListBlobContainerLst(usrUser);
            cntContenedores.usrUser = usrUser;
            return View(cntContenedores);
        }

        [HttpPost]
        public JsonResult sveContainer(string name)
        {
            
            //var blobStorage = new BlobStorage();
            var cntContainer = blobStorage.fntCreateBlobContainer(
                            new Container()
                            {
                                fntNameStr = name,
                                fntCompanyStr = usrUser.fntCompanyStr
                            });
            Mensaje mns = new Mensaje();
            mns.mensaje = "Contenedor Guardado";


            return Json(mns);
        }

        [HttpPost]
        public JsonResult dltContainer(string nombre)
        {
            ClsContainer cntContenedores = new ClsContainer();

            cntContenedores.lstContainers = blobStorage.fntListBlobContainerLst(usrUser);

            var cntContainer = blobStorage.fntDeleteContainer(cntContenedores.lstContainers.FirstOrDefault(x => x.fntFullNameStr == nombre), usrUser);

            return Json(new
            {
                Result = "ERROR",
                Message = "Formato no válido"
            });
        }
        public Container buscarContainer(string strNombreContainer)
        {
            ClsContainer cntContenedores = new ClsContainer();
            Container cntTemp = new Container();

            cntContenedores.lstContainers = blobStorage.fntListBlobContainerLst(usrUser);
            foreach (var item in cntContenedores.lstContainers)
            {
                if (strNombreContainer.Equals(item.fntFullNameStr))
                {
                    cntTemp = item;
                }

            }
            return cntTemp;
        }

        public ActionResult DeleteContainer()
        {
            ClsContainer cntContenedores = new ClsContainer();

            cntContenedores.lstContainers = blobStorage.fntListBlobContainerLst(usrUser);
            cntContenedores.usrUser = usrUser;
            return PartialView("_DeleteContainer", cntContenedores);
        }
        
        public ActionResult Upload()
        {
            ClsContainer cntContenedores = new ClsContainer();

            cntContenedores.lstContainers = blobStorage.fntListBlobContainerLst(usrUser);
            cntContenedores.usrUser = usrUser;
            return PartialView("_Upload", cntContenedores);
        }

        public ActionResult Browser()
        {
            return PartialView("_Browser");
        }

        [HttpPost]
        public ActionResult fntUploadFiles(List<DocumentUpload> lstDocuments)
        {
            //Create directory
            fntCreateDirectory();
            // Upload files for each container
            foreach (var strContainer in lstDocuments.Select(x => x.Container).Distinct())
            {
                var cntEncontrado = buscarContainer(strContainer);

                //guardar cada atributo en la clase fileupload
                var lstFileUpload = from doc in lstDocuments.Where(l => l.Container.Equals(strContainer))
                                    select (new FileUpload()
                                    {
                                        fntNameFileStr = doc.AlterName+"."+doc.Extension,
                                        fntApplicationStr = doc.Application,
                                        fntCompanyStr = doc.Company,
                                        fntLocalPathStr = doc.Path,
                                        fntMetadataDct = doc.AsDictionary()
                                    });

                //Upload documents to azure storage
                var lstBlobsUploads = blobStorage.fntUploadBlobs(cntEncontrado, lstFileUpload, usrUser);

                //Index pdf documents in list lstBlobs
                lstBlobsUploads = new AzureStorage.Search.IndexFiles().AddDocumentsToIndex(lstBlobsUploads, usrUser);
            }
            return Json("Sus archivos se están subiendo, le confirmaremos con un mensaje cuando hayan sido subidos con éxito");
        }
        [HttpPost]
        public JsonResult fntAzureSearch(clsDocumentPdfSearch parameters, int? page) {
            IndexFiles indexFiles = new IndexFiles();
            List<Enlace>lstTempEnlaces= new List<Enlace>();
            ClsContainer cntContenedores = new ClsContainer();
            try
            {
                var lstResult = indexFiles.SearchDocuments(parameters, usrUser);
                foreach (var item in lstResult)
                {
                    lstTempEnlaces.Add(new Enlace
                    {
                        alterName = item.AlterName,
                        referencialMonth = item.ReferencialMonth,
                        referencialYear = item.ReferencialYear,
                        container = item.Container,
                        application = item.Application,
                        module = item.Module,
                        documentUploadAt = item.DocumentUploadAt,
                        page = item.Page,
                        uri = fntReturnUri(item.Container, item.FullName, item.Page)
                    });
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                cntContenedores.lstEnlaces = lstTempEnlaces;
            }

            return Json(cntContenedores.lstEnlaces);
        }
        
        public string fntReturnUri(string strContainerName, string strBlobFullName, int intPageNumber) {
            var strUri = blobStorage.fntGetBlobSasUri(strContainerName, strBlobFullName, intPageNumber);
            return strUri;
        }

        public void fntCreateDirectory() {
            string strPath = ConfigurationManager.AppSettings["pathDirectory"].ToString();
            try
            {
                // Determine whether the directory exists.
                if (!Directory.Exists(strPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(strPath);
                }
              
            }
            catch (Exception e)
            {
                var stringMessage = e.ToString();
            }
        }
    }

}