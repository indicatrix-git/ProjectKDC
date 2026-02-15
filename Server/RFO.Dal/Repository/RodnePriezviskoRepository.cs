using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ditec.RIS.SysFra.DataAccess.Infrastructure;
using Ditec.SysFra.DataTypes.Infrastructure;
using Ditec.SysFra.Infrastructure.Impl;
using Ditec.SysFra.NhSql.Dal;
using LinFu.IoC.Configuration;
using NHibernate;

using Ditec.RIS.RFO.Dol;


namespace Ditec.RIS.RFO.Dal.Repository
{
	
  [Implements(typeof(ICRUD<RodnePriezvisko>), ServiceName = "RodnePriezvisko")]
  [Implements(typeof(ICRUDTransaction<RodnePriezvisko>), ServiceName = "RodnePriezvisko")]
  [Implements(typeof(IBrowse<RodnePriezvisko>), ServiceName = "RodnePriezvisko")]
  [Implements(typeof(IPagedBrowse<RodnePriezvisko>), ServiceName = "RodnePriezvisko")]
    [Implements(typeof(RodnePriezviskoRepository))]
	public partial class RodnePriezviskoRepository : DataAccessBase, ICRUD<RodnePriezvisko>, ICRUDTransaction<RodnePriezvisko>, IBrowse<RodnePriezvisko>, IPagedBrowse<RodnePriezvisko>
    {

		#region Create

		private const string RodnePriezviskoCreateQuery = "RodnePriezviskoCreate";

		public RodnePriezvisko Create(RodnePriezvisko dataObject)
		{
			try
			{
				// vytvori sa databazova session 
				using (ISession session = SessionProvider.OpenSession())
				{
					return Create(dataObject, session);
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 101);
			}
		}

		public RodnePriezvisko Create(RodnePriezvisko dataObject, ISession session)
		{
			try
			{
				RodnePriezvisko entry = (RodnePriezvisko)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(RodnePriezviskoCreateQuery);
				// vstupne parametre procedury sa naplnia udajmy z objektu 
				query.SetProperties(entry);
				query.SetParameter("TransakciaId", this.TransactionID);
				// zapis udajov do databazy 
                return query.UniqueResult<RodnePriezvisko>();
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 111);
			}
		}

		#endregion Create
		#region Update

		private const string RodnePriezviskoUpdateQuery = "RodnePriezviskoUpdate";

		public RodnePriezvisko Update(RodnePriezvisko dataObject)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					return Update(dataObject, session);
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 102);
			}
			return dataObject;
		}

		public RodnePriezvisko Update(RodnePriezvisko dataObject, ISession session)
		{
			try
			{
				RodnePriezvisko entry = (RodnePriezvisko)dataObject;
				IQuery query = session.GetNamedQuery(RodnePriezviskoUpdateQuery);
				query.SetProperties(entry);
				query.SetParameter("TransakciaId", this.TransactionID);
                query.SetParameter("TRANSAKCIA_ID_OLD", dataObject.TransakciaId);
				query.ExecuteUpdate();

				dataObject.TransakciaId = this.TransactionID as Guid?;
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 112);
			}
			return dataObject;
		}

		#endregion Update

		#region Delete

		private const string RodnePriezviskoDeleteQuery = "RodnePriezviskoDelete";

		public void Delete(RodnePriezvisko dataObject)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					Delete(dataObject, session);
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 103);
			}
		}

		public void Delete(RodnePriezvisko dataObject, ISession session)
		{
			try
			{
				RodnePriezvisko entry = (RodnePriezvisko)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(RodnePriezviskoDeleteQuery);
				//entry.Stav = -1;
				// vstupne parametre procedury sa naplnia udajmi z objektu 
				query.SetProperties(entry);
                query.SetParameter("TransakciaZruseneId", this.TransactionID);

				// zapis udajov do databazy 
				query.ExecuteUpdate();
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 113);
			}
		}

		#endregion Delete

		#region Get

		private const string RodnePriezviskoGetQuery = "RodnePriezviskoGet";

		public RodnePriezvisko Read(object dataObject)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(RodnePriezviskoGetQuery);
					query.SetProperties((RodnePriezviskoFilterCriteria)dataObject);
					var response = query.UniqueResult<RodnePriezvisko>();
					return (RodnePriezvisko)response;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
			}
		}

		#endregion Get

		#region Browse

		private const string RodnePriezviskoListQuery = "RodnePriezviskoList";
        
		public IList<RodnePriezvisko> Browse(object criteria)
		{
			var response = new List<RodnePriezvisko>();
			try
			{
				var filterCriteria = criteria as RodnePriezviskoFilterCriteria;
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(RodnePriezviskoListQuery);
					query.SetProperties(filterCriteria);
					
					if (query.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if(filterCriteria.DatumPlatnosti.HasValue)
							query.SetParameter("DATUM_SIMULACIE", filterCriteria.DatumPlatnosti.Value);
						else
							query.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

					response = query.List<RodnePriezvisko>().ToList();
					return response;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 105);
			}
		}

		#endregion Browse
		
		#region PagedBrowse

		private const string RodnePriezviskoBrowseCntQuery = "RodnePriezviskoBrowseCnt";
        private const string RodnePriezviskoBrowsePgQuery = "RodnePriezviskoBrowsePg";
        
		public IList<RodnePriezvisko> PagedBrowse(ref RequestResult<List<RodnePriezvisko>> requestResult, object filterCriteria)
		{
			var response = new List<RodnePriezvisko>();
			var criteria = (RodnePriezviskoFilterCriteria)filterCriteria;

            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery queryCnt = session.GetNamedQuery(RodnePriezviskoBrowseCntQuery);
                    queryCnt.SetProperties(criteria);
					if (queryCnt.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if (criteria.DatumPlatnosti.HasValue)
							queryCnt.SetParameter("DATUM_SIMULACIE", criteria.DatumPlatnosti.Value);
						else
							queryCnt.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

                    ((RodnePriezviskoFilterCriteria)filterCriteria).PagingInfo.TotalRecords = queryCnt.UniqueResult<int>();

                    IQuery query = session.GetNamedQuery(RodnePriezviskoBrowsePgQuery);
                    query.SetProperties(criteria);

					 if (((RodnePriezviskoFilterCriteria)filterCriteria).PagingInfo.TotalRecords > 0 && criteria.PagingInfo.CurrentPage == 0)
                        criteria.PagingInfo.CurrentPage = 1;

                    query.SetParameter("CurrentPage", criteria.PagingInfo.CurrentPage);
                    query.SetParameter("RecordsPerPage", criteria.PagingInfo.RecordsPerPage);
					if (query.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if (criteria.DatumPlatnosti.HasValue)
							query.SetParameter("DATUM_SIMULACIE", criteria.DatumPlatnosti.Value);
						else
							query.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}
                    response = query.List<RodnePriezvisko>().ToList();

                    // aktualizuje sa informacia o strankovanom zozname v obalke RequestResult 
                    requestResult.PagingInfo = criteria.PagingInfo;

                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 106);
            }
		}

		#endregion PagedBrowse
	}
}