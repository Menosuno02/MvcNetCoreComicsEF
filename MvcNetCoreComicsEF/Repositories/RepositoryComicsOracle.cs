using Microsoft.EntityFrameworkCore;
using MvcNetCoreComicsEF.Data;
using MvcNetCoreComicsEF.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

#region VIEWS Y PROCEDURES
/*
CREATE OR REPLACE PROCEDURE SP_GET_COMICS
(P_CURSOR_COMICS OUT SYS_REFCURSOR)
AS
BEGIN
  OPEN P_CURSOR_COMICS FOR
  SELECT *
  FROM COMICS;
END;

CREATE OR REPLACE PROCEDURE SP_FIND_COMIC
(P_CURSOR_COMIC OUT SYS_REFCURSOR,
P_IDCOMIC COMICS.IDCOMIC%TYPE)
AS
BEGIN
  OPEN P_CURSOR_COMIC FOR
  SELECT *
  FROM COMICS
  WHERE IDCOMIC = P_IDCOMIC;
END;

CREATE OR REPLACE PROCEDURE SP_CREATE_COMIC
(P_NOMBRE COMICS.NOMBRE%TYPE,
P_IMAGEN COMICS.IMAGEN%TYPE,
P_DESC COMICS.DESCRIPCION%TYPE)
AS
P_IDCOMIC COMICS.IDCOMIC%TYPE;
BEGIN
  SELECT MAX(IDCOMIC) + 1 INTO P_IDCOMIC
  FROM COMICS;
  INSERT INTO COMICS VALUES
  (P_IDCOMIC, P_NOMBRE, P_IMAGEN, P_DESC);
  COMMIT;
END;

CREATE OR REPLACE PROCEDURE SP_DELETE_COMIC
(P_IDCOMIC COMICS.IDCOMIC%TYPE)
AS
BEGIN
  DELETE FROM COMICS
  WHERE IDCOMIC = P_IDCOMIC;
  COMMIT;
END;
*/
#endregion

namespace MvcNetCoreComicsEF.Repositories
{
    public class RepositoryComicsOracle : IRepositoryComics
    {
        private ComicContext context;

        public RepositoryComicsOracle(ComicContext context)
        {
            this.context = context;
        }

        public async Task CreateComicAsync(Comic comic)
        {
            string sql = "begin ";
            sql += "SP_CREATE_COMIC(:P_NOMBRE, :P_IMAGEN, :P_DESC);";
            sql += "end;";
            OracleParameter paramNombre = new OracleParameter(":P_NOMBRE", comic.Nombre);
            OracleParameter paramImagen = new OracleParameter(":P_IMAGEN", comic.Imagen);
            OracleParameter paramDesc = new OracleParameter(":P_DESC", comic.Descripcion);
            await this.context.Database.ExecuteSqlRawAsync(sql, paramNombre,
                paramImagen, paramDesc);

        }

        public async Task DeleteComicAsync(int idComic)
        {
            string sql = "begin ";
            sql += "SP_DELETE_COMIC(:P_IDCOMIC);";
            sql += "end;";
            OracleParameter paramId = new OracleParameter(":P_IDCOMIC", idComic);
            await this.context.Database.ExecuteSqlRawAsync(sql, paramId);
        }

        public async Task<Comic> FindComicAsync(int idComic)
        {
            string sql = "begin ";
            sql += "SP_FIND_COMIC(:P_CURSOR_COMIC, :P_IDCOMIC);";
            sql += "end;";
            OracleParameter paramCursor = new OracleParameter();
            paramCursor.ParameterName = ":P_CURSOR_COMIC";
            paramCursor.Value = null;
            paramCursor.Direction = ParameterDirection.Output;
            paramCursor.OracleDbType = OracleDbType.RefCursor;
            OracleParameter paramId = new OracleParameter(":P_IDCOMIC", idComic);
            var consulta = this.context.Comics.FromSqlRaw(sql, paramCursor, paramId);
            Comic comic = consulta.AsEnumerable().FirstOrDefault();
            return comic;
        }

        public async Task<List<Comic>> GetComicsAsync()
        {
            string sql = "begin ";
            sql += "SP_GET_COMICS(:P_CURSOR_COMICS);";
            sql += "end;";
            OracleParameter paramCursor = new OracleParameter();
            paramCursor.ParameterName = ":P_CURSOR_COMICS";
            paramCursor.Value = null;
            paramCursor.Direction = ParameterDirection.Output;
            paramCursor.OracleDbType = OracleDbType.RefCursor;
            var consulta = this.context.Comics.FromSqlRaw(sql, paramCursor);
            return await consulta.ToListAsync();
        }
    }
}
