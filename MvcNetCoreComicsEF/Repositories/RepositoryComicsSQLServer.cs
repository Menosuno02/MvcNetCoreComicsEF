using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreComicsEF.Data;
using MvcNetCoreComicsEF.Models;

#region VIEWS Y PROCEDURES
/*
CREATE OR ALTER PROCEDURE SP_GET_COMICS
AS
	SELECT *
	FROM COMICS
GO

CREATE OR ALTER PROCEDURE SP_FIND_COMIC
(@IDCOMIC INT)
AS
	SELECT *
	FROM COMICS
	WHERE IDCOMIC = @IDCOMIC;
GO

CREATE OR ALTER PROCEDURE SP_CREATE_COMIC
(@NOMBRE NVARCHAR(150), @IMAGEN NVARCHAR(600),
@DESCRIPCION NVARCHAR(600))
AS
	DECLARE @IDCOMIC INT
	SELECT @IDCOMIC = MAX(IDCOMIC) + 1
	FROM COMICS
	INSERT INTO COMICS VALUES
	(@IDCOMIC, @NOMBRE, @IMAGEN, @DESCRIPCION)
GO

CREATE OR ALTER PROCEDURE SP_DELETE_COMIC
(@IDCOMIC INT)
AS
	DELETE FROM COMICS
	WHERE IDCOMIC = @IDCOMIC
GO
*/
#endregion

namespace MvcNetCoreComicsEF.Repositories
{
    public class RepositoryComicsSQLServer : IRepositoryComics
    {

        private ComicContext context;

        public RepositoryComicsSQLServer(ComicContext context)
        {
            this.context = context;
        }

        public async Task CreateComicAsync(Comic comic)
        {
            string sql = "SP_CREATE_COMIC @NOMBRE, @IMAGEN, @DESCRIPCION";
            SqlParameter paramNombre = new SqlParameter("@NOMBRE", comic.Nombre);
            SqlParameter paramImagen = new SqlParameter("@IMAGEN", comic.Imagen);
            SqlParameter paramDesc = new SqlParameter("@DESCRIPCION", comic.Descripcion);
            await this.context.Database.ExecuteSqlRawAsync
                (sql, paramNombre, paramImagen, paramDesc);
        }

        public async Task DeleteComicAsync(int idComic)
        {
            string sql = "SP_DELETE_COMIC @IDCOMIC";
            SqlParameter paramId = new SqlParameter("@IDCOMIC", idComic);
            await this.context.Database.ExecuteSqlRawAsync(sql, paramId);
        }

        public async Task<Comic> FindComicAsync(int idComic)
        {
            string sql = "SP_FIND_COMIC @IDCOMIC";
            SqlParameter paramId = new SqlParameter("@IDCOMIC", idComic);
            var consulta = this.context.Comics.FromSqlRaw(sql, paramId);
            Comic comic = consulta.AsEnumerable().FirstOrDefault();
            return comic;
        }

        public async Task<List<Comic>> GetComicsAsync()
        {
            string sql = "SP_GET_COMICS";
            var consulta = this.context.Comics.FromSqlRaw(sql);
            return await consulta.ToListAsync();
        }
    }
}
