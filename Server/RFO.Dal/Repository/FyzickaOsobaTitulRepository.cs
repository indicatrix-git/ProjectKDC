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
	
  [Implements(typeof(ICRUD<FyzickaOsobaTitul>), ServiceName = "FyzickaOsobaTitul")]
  [Implements(typeof(ICRUDTransaction<FyzickaOsobaTitul>), ServiceName = "FyzickaOsobaTitul")]
  [Implements(typeof(IBrowse<FyzickaOsobaTitul>), ServiceName = "FyzickaOsobaTitul")]
  [Implements(typeof(IPagedBrowse<FyzickaOsobaTitul>), ServiceName = "FyzickaOsobaTitul")]
    [Implements(typeof(FyzickaOsobaTitulRepository))]
	public partial class FyzickaOsobaTitulRepository : DataAccessBase, ICRUD<FyzickaOsobaTitul>, ICRUDTransaction<FyzickaOsobaTitul>, IBrowse<FyzickaOsobaTitul>, IPagedBrowse<FyzickaOsobaTitul>
    {

		#region Create

		private const string FyzickaOsobaTitulCreateQuery = "FyzickaOsobaTitulFOCreate";

		public FyzickaOsobaTitul Create(FyzickaOsobaTitul dataObject)
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

		public FyzickaOsobaTitul Create(FyzickaOsobaTitul dataObject, ISession session)
		{
			try
			{
				FyzickaOsobaTitul entry = (FyzickaOsobaTitul)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(FyzickaOsobaTitulCreateQuery);
				// vstupne parametre procedury sa naplnia udajmy z objektu 
				query.SetProperties(entry);
				query.SetParameter("TransakciaId", this.TransactionID);
				// zapis udajov do databazy 
                return query.UniqueResult<FyzickaOsobaTitul>();
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 111);
			}
		}

		#endregion Create
		#region Update

		private const string FyzickaOsobaTitulUpdateQuery = "FyzickaOsobaTitulFOUpdate";

		public FyzickaOsobaTitul Update(FyzickaOsobaTitul dataObject)
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

		public FyzickaOsobaTitul Update(FyzickaOsobaTitul dataObject, ISession session)
		{
			try
			{
				FyzickaOsobaTitul entry = (FyzickaOsobaTitul)dataObject;
				IQuery query = session.GetNamedQuery(FyzickaOsobaTitulUpdateQuery);
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

		private const string FyzickaOsobaTitulDeleteQuery = "FyzickaOsobaTitulFODelete";

		public void Delete(FyzickaOsobaTitul dataObject)
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

		public void Delete(FyzickaOsobaTitul dataObject, ISession session)
		{
			try
			{
				FyzickaOsobaTitul entry = (FyzickaOsobaTitul)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(FyzickaOsobaTitulDeleteQuery);
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

		private const string FyzickaOsobaTitulGetQuery = "FyzickaOsobaTitulFOGet";

		public FyzickaOsobaTitul Read(object dataObject)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(FyzickaOsobaTitulGetQuery);
					query.SetProperties((FyzickaOsobaTitulFilterCriteria)dataObject);
					var response = query.UniqueResult<FyzickaOsobaTitul>();
					return (FyzickaOsobaTitul)response;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
			}
		}

		#endregion Get

		#region Browse

		private const string FyzickaOsobaTitulListQuery = "FyzickaOsobaTitulFOList";
        
		public IList<FyzickaOsobaTitul> Browse(object criteria)
		{
			var response = new List<FyzickaOsobaTitul>();
			try
			{
				var filterCriteria = criteria as FyzickaOsobaTitulFilterCriteria;
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(FyzickaOsobaTitulListQuery);
					query.SetProperties(filterCriteria);
					
					if (query.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if(filterCriteria.DatumPlatnosti.HasValue)
							query.SetParameter("DATUM_SIMULACIE", filterCriteria.DatumPlatnosti.Value);
						else
							query.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

					response = query.List<FyzickaOsobaTitul>().ToList();
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

		private const string FyzickaOsobaTitulBrowseCntQuery = "FyzickaOsobaTitulFOBrowseCnt";
        private const string FyzickaOsobaTitulBrowsePgQuery = "FyzickaOsobaTitulFOBrowsePg";
        
		public IList<FyzickaOsobaTitul> PagedBrowse(ref RequestResult<List<FyzickaOsobaTitul>> requestResult, object filterCriteria)
		{
			var response = new List<FyzickaOsobaTitul>();
			var criteria = (FyzickaOsobaTitulFilterCriteria)filterCriteria;

            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery queryCnt = session.GetNamedQuery(FyzickaOsobaTitulBrowseCntQuery);
                    queryCnt.SetProperties(criteria);
					if (queryCnt.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if (criteria.DatumPlatnosti.HasValue)
							queryCnt.SetParameter("DATUM_SIMULACIE", criteria.DatumPlatnosti.Value);
						else
							queryCnt.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

                    ((FyzickaOsobaTitulFilterCriteria)filterCriteria).PagingInfo.TotalRecords = queryCnt.UniqueResult<int>();

                    IQuery query = session.GetNamedQuery(FyzickaOsobaTitulBrowsePgQuery);
                    query.SetProperties(criteria);

					 if (((FyzickaOsobaTitulFilterCriteria)filterCriteria).PagingInfo.TotalRecords > 0 && criteria.PagingInfo.CurrentPage == 0)
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
                    response = query.List<FyzickaOsobaTitul>().ToList();

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