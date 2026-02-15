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
	
  [Implements(typeof(ICRUD<ZakazPobytu>), ServiceName = "ZakazPobytu")]
  [Implements(typeof(ICRUDTransaction<ZakazPobytu>), ServiceName = "ZakazPobytu")]
  [Implements(typeof(IBrowse<ZakazPobytu>), ServiceName = "ZakazPobytu")]
  [Implements(typeof(IPagedBrowse<ZakazPobytu>), ServiceName = "ZakazPobytu")]
    [Implements(typeof(ZakazPobytuRepository))]
	public partial class ZakazPobytuRepository : DataAccessBase, ICRUD<ZakazPobytu>, ICRUDTransaction<ZakazPobytu>, IBrowse<ZakazPobytu>, IPagedBrowse<ZakazPobytu>
    {

		#region Create

		private const string ZakazPobytuCreateQuery = "ZakazPobytuCreate";

		public ZakazPobytu Create(ZakazPobytu dataObject)
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

		public ZakazPobytu Create(ZakazPobytu dataObject, ISession session)
		{
			try
			{
				ZakazPobytu entry = (ZakazPobytu)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(ZakazPobytuCreateQuery);
				// vstupne parametre procedury sa naplnia udajmy z objektu 
				query.SetProperties(entry);
				query.SetParameter("TransakciaId", this.TransactionID);
				// zapis udajov do databazy 
                return query.UniqueResult<ZakazPobytu>();
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 111);
			}
		}

		#endregion Create

		#region Update

		private const string ZakazPobytuUpdateQuery = "ZakazPobytuUpdate";

		public ZakazPobytu Update(ZakazPobytu dataObject)
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

		public ZakazPobytu Update(ZakazPobytu dataObject, ISession session)
		{
			try
			{
				ZakazPobytu entry = (ZakazPobytu)dataObject;
				IQuery query = session.GetNamedQuery(ZakazPobytuUpdateQuery);
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

		private const string ZakazPobytuDeleteQuery = "ZakazPobytuDelete";

		public void Delete(ZakazPobytu dataObject)
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

		public void Delete(ZakazPobytu dataObject, ISession session)
		{
			try
			{
				ZakazPobytu entry = (ZakazPobytu)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(ZakazPobytuDeleteQuery);
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

		private const string ZakazPobytuGetQuery = "ZakazPobytuGet";

		public ZakazPobytu Read(object dataObject)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(ZakazPobytuGetQuery);
					query.SetProperties((ZakazPobytuFilterCriteria)dataObject);
					var response = query.UniqueResult<ZakazPobytu>();
					return (ZakazPobytu)response;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
			}
		}

		#endregion Get

		#region Browse

		private const string ZakazPobytuListQuery = "ZakazPobytuList";
        
		public IList<ZakazPobytu> Browse(object criteria)
		{
			var response = new List<ZakazPobytu>();
			try
			{
				var filterCriteria = criteria as ZakazPobytuFilterCriteria;
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(ZakazPobytuListQuery);
					query.SetProperties(filterCriteria);
					
					if (query.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if(filterCriteria.DatumPlatnosti.HasValue)
							query.SetParameter("DATUM_SIMULACIE", filterCriteria.DatumPlatnosti.Value);
						else
							query.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

					response = query.List<ZakazPobytu>().ToList();
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

		private const string ZakazPobytuBrowseCntQuery = "ZakazPobytuBrowseCnt";
        private const string ZakazPobytuBrowsePgQuery = "ZakazPobytuBrowsePg";
        
		public IList<ZakazPobytu> PagedBrowse(ref RequestResult<List<ZakazPobytu>> requestResult, object filterCriteria)
		{
			var response = new List<ZakazPobytu>();
			var criteria = (ZakazPobytuFilterCriteria)filterCriteria;

            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery queryCnt = session.GetNamedQuery(ZakazPobytuBrowseCntQuery);
                    queryCnt.SetProperties(criteria);
					if (queryCnt.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if (criteria.DatumPlatnosti.HasValue)
							queryCnt.SetParameter("DATUM_SIMULACIE", criteria.DatumPlatnosti.Value);
						else
							queryCnt.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

                    ((ZakazPobytuFilterCriteria)filterCriteria).PagingInfo.TotalRecords = queryCnt.UniqueResult<int>();

                    IQuery query = session.GetNamedQuery(ZakazPobytuBrowsePgQuery);
                    query.SetProperties(criteria);

					 if (((ZakazPobytuFilterCriteria)filterCriteria).PagingInfo.TotalRecords > 0 && criteria.PagingInfo.CurrentPage == 0)
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
                    response = query.List<ZakazPobytu>().ToList();

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