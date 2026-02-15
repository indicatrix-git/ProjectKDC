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
	
  [Implements(typeof(ICRUD<RegionMimoSr>), ServiceName = "RegionMimoSr")]
  [Implements(typeof(ICRUDTransaction<RegionMimoSr>), ServiceName = "RegionMimoSr")]
  [Implements(typeof(IBrowse<RegionMimoSr>), ServiceName = "RegionMimoSr")]
  [Implements(typeof(IPagedBrowse<RegionMimoSr>), ServiceName = "RegionMimoSr")]
    [Implements(typeof(RegionMimoSrRepository))]
	public partial class RegionMimoSrRepository : DataAccessBase, ICRUD<RegionMimoSr>, ICRUDTransaction<RegionMimoSr>, IBrowse<RegionMimoSr>, IPagedBrowse<RegionMimoSr>
    {

		#region Create

		private const string RegionMimoSrCreateQuery = "RegionMimoSrCreate";

		public RegionMimoSr Create(RegionMimoSr dataObject)
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

		public RegionMimoSr Create(RegionMimoSr dataObject, ISession session)
		{
			try
			{
				RegionMimoSr entry = (RegionMimoSr)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(RegionMimoSrCreateQuery);
				// vstupne parametre procedury sa naplnia udajmy z objektu 
				query.SetProperties(entry);
				query.SetParameter("TransakciaId", this.TransactionID);
				// zapis udajov do databazy 
                return query.UniqueResult<RegionMimoSr>();
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 111);
			}
		}

		#endregion Create

		#region Update

		private const string RegionMimoSrUpdateQuery = "RegionMimoSrUpdate";

		public RegionMimoSr Update(RegionMimoSr dataObject)
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

		public RegionMimoSr Update(RegionMimoSr dataObject, ISession session)
		{
			try
			{
				RegionMimoSr entry = (RegionMimoSr)dataObject;
				IQuery query = session.GetNamedQuery(RegionMimoSrUpdateQuery);
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

		private const string RegionMimoSrDeleteQuery = "RegionMimoSrDelete";

		public void Delete(RegionMimoSr dataObject)
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

		public void Delete(RegionMimoSr dataObject, ISession session)
		{
			try
			{
				RegionMimoSr entry = (RegionMimoSr)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(RegionMimoSrDeleteQuery);
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

		private const string RegionMimoSrGetQuery = "RegionMimoSrGet";

		public RegionMimoSr Read(object dataObject)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(RegionMimoSrGetQuery);
					query.SetProperties((RegionMimoSrFilterCriteria)dataObject);
					var response = query.UniqueResult<RegionMimoSr>();
					return (RegionMimoSr)response;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
			}
		}

		#endregion Get

		#region Browse

		private const string RegionMimoSrListQuery = "RegionMimoSrList";
        
		public IList<RegionMimoSr> Browse(object criteria)
		{
			var response = new List<RegionMimoSr>();
			try
			{
				var filterCriteria = criteria as RegionMimoSrFilterCriteria;
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(RegionMimoSrListQuery);
					query.SetProperties(filterCriteria);
					
					if (query.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if(filterCriteria.DatumPlatnosti.HasValue)
							query.SetParameter("DATUM_SIMULACIE", filterCriteria.DatumPlatnosti.Value);
						else
							query.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

					response = query.List<RegionMimoSr>().ToList();
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

		private const string RegionMimoSrBrowseCntQuery = "RegionMimoSrBrowseCnt";
        private const string RegionMimoSrBrowsePgQuery = "RegionMimoSrBrowsePg";
        
		public IList<RegionMimoSr> PagedBrowse(ref RequestResult<List<RegionMimoSr>> requestResult, object filterCriteria)
		{
			var response = new List<RegionMimoSr>();
			var criteria = (RegionMimoSrFilterCriteria)filterCriteria;

            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery queryCnt = session.GetNamedQuery(RegionMimoSrBrowseCntQuery);
                    queryCnt.SetProperties(criteria);
					if (queryCnt.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if (criteria.DatumPlatnosti.HasValue)
							queryCnt.SetParameter("DATUM_SIMULACIE", criteria.DatumPlatnosti.Value);
						else
							queryCnt.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

                    ((RegionMimoSrFilterCriteria)filterCriteria).PagingInfo.TotalRecords = queryCnt.UniqueResult<int>();

                    IQuery query = session.GetNamedQuery(RegionMimoSrBrowsePgQuery);
                    query.SetProperties(criteria);

					 if (((RegionMimoSrFilterCriteria)filterCriteria).PagingInfo.TotalRecords > 0 && criteria.PagingInfo.CurrentPage == 0)
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
                    response = query.List<RegionMimoSr>().ToList();

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