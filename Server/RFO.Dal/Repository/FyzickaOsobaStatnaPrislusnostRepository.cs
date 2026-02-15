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
	
  [Implements(typeof(ICRUD<FyzickaOsobaStatnaPrislusnost>), ServiceName = "FyzickaOsobaStatnaPrislusnost")]
  [Implements(typeof(ICRUDTransaction<FyzickaOsobaStatnaPrislusnost>), ServiceName = "FyzickaOsobaStatnaPrislusnost")]
  [Implements(typeof(IBrowse<FyzickaOsobaStatnaPrislusnost>), ServiceName = "FyzickaOsobaStatnaPrislusnost")]
  [Implements(typeof(IPagedBrowse<FyzickaOsobaStatnaPrislusnost>), ServiceName = "FyzickaOsobaStatnaPrislusnost")]
    [Implements(typeof(FyzickaOsobaStatnaPrislusnostRepository))]
	public partial class FyzickaOsobaStatnaPrislusnostRepository : DataAccessBase, ICRUD<FyzickaOsobaStatnaPrislusnost>, ICRUDTransaction<FyzickaOsobaStatnaPrislusnost>, IBrowse<FyzickaOsobaStatnaPrislusnost>, IPagedBrowse<FyzickaOsobaStatnaPrislusnost>
    {

		#region Create

		private const string FyzickaOsobaStatnaPrislusnostCreateQuery = "FyzickaOsobaStatnaPrislusnostCreate";

		public FyzickaOsobaStatnaPrislusnost Create(FyzickaOsobaStatnaPrislusnost dataObject)
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

		public FyzickaOsobaStatnaPrislusnost Create(FyzickaOsobaStatnaPrislusnost dataObject, ISession session)
		{
			try
			{
				FyzickaOsobaStatnaPrislusnost entry = (FyzickaOsobaStatnaPrislusnost)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(FyzickaOsobaStatnaPrislusnostCreateQuery);
				// vstupne parametre procedury sa naplnia udajmy z objektu 
				query.SetProperties(entry);
				query.SetParameter("TransakciaId", this.TransactionID);
				// zapis udajov do databazy 
                return query.UniqueResult<FyzickaOsobaStatnaPrislusnost>();
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 111);
			}
		}

		#endregion Create
		#region Update

		private const string FyzickaOsobaStatnaPrislusnostUpdateQuery = "FyzickaOsobaStatnaPrislusnostUpdate";

		public FyzickaOsobaStatnaPrislusnost Update(FyzickaOsobaStatnaPrislusnost dataObject)
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

		public FyzickaOsobaStatnaPrislusnost Update(FyzickaOsobaStatnaPrislusnost dataObject, ISession session)
		{
			try
			{
				FyzickaOsobaStatnaPrislusnost entry = (FyzickaOsobaStatnaPrislusnost)dataObject;
				IQuery query = session.GetNamedQuery(FyzickaOsobaStatnaPrislusnostUpdateQuery);
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

		private const string FyzickaOsobaStatnaPrislusnostDeleteQuery = "FyzickaOsobaStatnaPrislusnostDelete";

		public void Delete(FyzickaOsobaStatnaPrislusnost dataObject)
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

		public void Delete(FyzickaOsobaStatnaPrislusnost dataObject, ISession session)
		{
			try
			{
				FyzickaOsobaStatnaPrislusnost entry = (FyzickaOsobaStatnaPrislusnost)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(FyzickaOsobaStatnaPrislusnostDeleteQuery);
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

		private const string FyzickaOsobaStatnaPrislusnostGetQuery = "FyzickaOsobaStatnaPrislusnostGet";

		public FyzickaOsobaStatnaPrislusnost Read(object dataObject)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(FyzickaOsobaStatnaPrislusnostGetQuery);
					query.SetProperties((FyzickaOsobaStatnaPrislusnostFilterCriteria)dataObject);
					var response = query.UniqueResult<FyzickaOsobaStatnaPrislusnost>();
					return (FyzickaOsobaStatnaPrislusnost)response;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
			}
		}

		#endregion Get

		#region Browse

		private const string FyzickaOsobaStatnaPrislusnostListQuery = "FyzickaOsobaStatnaPrislusnostList";
        
		public IList<FyzickaOsobaStatnaPrislusnost> Browse(object criteria)
		{
			var response = new List<FyzickaOsobaStatnaPrislusnost>();
			try
			{
				var filterCriteria = criteria as FyzickaOsobaStatnaPrislusnostFilterCriteria;
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(FyzickaOsobaStatnaPrislusnostListQuery);
					query.SetProperties(filterCriteria);
					
					if (query.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if(filterCriteria.DatumPlatnosti.HasValue)
							query.SetParameter("DATUM_SIMULACIE", filterCriteria.DatumPlatnosti.Value);
						else
							query.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

					response = query.List<FyzickaOsobaStatnaPrislusnost>().ToList();
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

		private const string FyzickaOsobaStatnaPrislusnostBrowseCntQuery = "FyzickaOsobaStatnaPrislusnostBrowseCnt";
        private const string FyzickaOsobaStatnaPrislusnostBrowsePgQuery = "FyzickaOsobaStatnaPrislusnostBrowsePg";
        
		public IList<FyzickaOsobaStatnaPrislusnost> PagedBrowse(ref RequestResult<List<FyzickaOsobaStatnaPrislusnost>> requestResult, object filterCriteria)
		{
			var response = new List<FyzickaOsobaStatnaPrislusnost>();
			var criteria = (FyzickaOsobaStatnaPrislusnostFilterCriteria)filterCriteria;

            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery queryCnt = session.GetNamedQuery(FyzickaOsobaStatnaPrislusnostBrowseCntQuery);
                    queryCnt.SetProperties(criteria);
					if (queryCnt.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if (criteria.DatumPlatnosti.HasValue)
							queryCnt.SetParameter("DATUM_SIMULACIE", criteria.DatumPlatnosti.Value);
						else
							queryCnt.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

                    ((FyzickaOsobaStatnaPrislusnostFilterCriteria)filterCriteria).PagingInfo.TotalRecords = queryCnt.UniqueResult<int>();

                    IQuery query = session.GetNamedQuery(FyzickaOsobaStatnaPrislusnostBrowsePgQuery);
                    query.SetProperties(criteria);

					 if (((FyzickaOsobaStatnaPrislusnostFilterCriteria)filterCriteria).PagingInfo.TotalRecords > 0 && criteria.PagingInfo.CurrentPage == 0)
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
                    response = query.List<FyzickaOsobaStatnaPrislusnost>().ToList();

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