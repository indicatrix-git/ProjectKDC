using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Runtime.Serialization;


namespace Ditec.RIS.RFO.Dol
{
	[DataContract()]
	public partial class UdajePobytu  : BaseDataMember
	{
		
        private Guid _FyzickaOsobaId; 
        [DataMember] 
        public virtual Guid FyzickaOsobaId  
        {   
            get   
            {  
                return _FyzickaOsobaId; 
            } 
            set 
            {
                if (value != _FyzickaOsobaId) 
                {
                    _FyzickaOsobaId = value; 
                    OnPropertyChanged("FyzickaOsobaId"); 
                } 
            } 
        } 

        private Guid _TypPobytuIk; 
        [DataMember] 
        public virtual Guid TypPobytuIk  
        {   
            get   
            {  
                return _TypPobytuIk; 
            } 
            set 
            {
                if (value != _TypPobytuIk) 
                {
                    _TypPobytuIk = value; 
                    OnPropertyChanged("TypPobytuIk"); 
                } 
            } 
        } 

        private Guid? _RfoOkresIk; 
        [DataMember] 
        public virtual Guid? RfoOkresIk  
        {   
            get   
            {  
                return _RfoOkresIk; 
            } 
            set 
            {
                if (value != _RfoOkresIk) 
                {
                    _RfoOkresIk = value; 
                    OnPropertyChanged("RfoOkresIk"); 
                } 
            } 
        } 

        private Guid? _RfoObecIk; 
        [DataMember] 
        public virtual Guid? RfoObecIk  
        {   
            get   
            {  
                return _RfoObecIk; 
            } 
            set 
            {
                if (value != _RfoObecIk) 
                {
                    _RfoObecIk = value; 
                    OnPropertyChanged("RfoObecIk"); 
                } 
            } 
        } 

        private Guid? _RfoStatIk; 
        [DataMember] 
        public virtual Guid? RfoStatIk  
        {   
            get   
            {  
                return _RfoStatIk; 
            } 
            set 
            {
                if (value != _RfoStatIk) 
                {
                    _RfoStatIk = value; 
                    OnPropertyChanged("RfoStatIk"); 
                } 
            } 
        } 

        private Guid? _RfoCastObceIk; 
        [DataMember] 
        public virtual Guid? RfoCastObceIk  
        {   
            get   
            {  
                return _RfoCastObceIk; 
            } 
            set 
            {
                if (value != _RfoCastObceIk) 
                {
                    _RfoCastObceIk = value; 
                    OnPropertyChanged("RfoCastObceIk"); 
                } 
            } 
        } 

        private Guid? _RfoUlicaIk; 
        [DataMember] 
        public virtual Guid? RfoUlicaIk  
        {   
            get   
            {  
                return _RfoUlicaIk; 
            } 
            set 
            {
                if (value != _RfoUlicaIk) 
                {
                    _RfoUlicaIk = value; 
                    OnPropertyChanged("RfoUlicaIk"); 
                } 
            } 
        } 

        private Boolean _MimoSr; 
        [DataMember] 
        public virtual Boolean MimoSr  
        {   
            get   
            {  
                return _MimoSr; 
            } 
            set 
            {
                if (value != _MimoSr) 
                {
                    _MimoSr = value; 
                    OnPropertyChanged("MimoSr"); 
                } 
            } 
        } 

        private String _Ulica; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(100)]
        public virtual String Ulica  
        {   
            get   
            {  
                return _Ulica; 
            } 
            set 
            {
                if (value != _Ulica) 
                {
                    _Ulica = value; 
                    OnPropertyChanged("Ulica"); 
                } 
            } 
        } 

        private String _OrientacneCislo; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(100)]
        public virtual String OrientacneCislo  
        {   
            get   
            {  
                return _OrientacneCislo; 
            } 
            set 
            {
                if (value != _OrientacneCislo) 
                {
                    _OrientacneCislo = value; 
                    OnPropertyChanged("OrientacneCislo"); 
                } 
            } 
        } 

        private Int32? _SupisneCislo; 
        [DataMember] 
        public virtual Int32? SupisneCislo  
        {   
            get   
            {  
                return _SupisneCislo; 
            } 
            set 
            {
                if (value != _SupisneCislo) 
                {
                    _SupisneCislo = value; 
                    OnPropertyChanged("SupisneCislo"); 
                } 
            } 
        } 

        private String _MiestoMimoSr; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(120)]
        public virtual String MiestoMimoSr  
        {   
            get   
            {  
                return _MiestoMimoSr; 
            } 
            set 
            {
                if (value != _MiestoMimoSr) 
                {
                    _MiestoMimoSr = value; 
                    OnPropertyChanged("MiestoMimoSr"); 
                } 
            } 
        } 

        private String _OkresMimoSr; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(255)]
        public virtual String OkresMimoSr  
        {   
            get   
            {  
                return _OkresMimoSr; 
            } 
            set 
            {
                if (value != _OkresMimoSr) 
                {
                    _OkresMimoSr = value; 
                    OnPropertyChanged("OkresMimoSr"); 
                } 
            } 
        } 

        private String _ObecMimoSr; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(255)]
        public virtual String ObecMimoSr  
        {   
            get   
            {  
                return _ObecMimoSr; 
            } 
            set 
            {
                if (value != _ObecMimoSr) 
                {
                    _ObecMimoSr = value; 
                    OnPropertyChanged("ObecMimoSr"); 
                } 
            } 
        } 

        private String _CastObceMimoSr; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(255)]
        public virtual String CastObceMimoSr  
        {   
            get   
            {  
                return _CastObceMimoSr; 
            } 
            set 
            {
                if (value != _CastObceMimoSr) 
                {
                    _CastObceMimoSr = value; 
                    OnPropertyChanged("CastObceMimoSr"); 
                } 
            } 
        } 

        private String _UlicaMimoSr; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(255)]
        public virtual String UlicaMimoSr  
        {   
            get   
            {  
                return _UlicaMimoSr; 
            } 
            set 
            {
                if (value != _UlicaMimoSr) 
                {
                    _UlicaMimoSr = value; 
                    OnPropertyChanged("UlicaMimoSr"); 
                } 
            } 
        } 

        private String _OrientacneCisloMimoSr; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(100)]
        public virtual String OrientacneCisloMimoSr  
        {   
            get   
            {  
                return _OrientacneCisloMimoSr; 
            } 
            set 
            {
                if (value != _OrientacneCisloMimoSr) 
                {
                    _OrientacneCisloMimoSr = value; 
                    OnPropertyChanged("OrientacneCisloMimoSr"); 
                } 
            } 
        } 

        private String _SupisneCisloMimoSr; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(100)]
        public virtual String SupisneCisloMimoSr  
        {   
            get   
            {  
                return _SupisneCisloMimoSr; 
            } 
            set 
            {
                if (value != _SupisneCisloMimoSr) 
                {
                    _SupisneCisloMimoSr = value; 
                    OnPropertyChanged("SupisneCisloMimoSr"); 
                } 
            } 
        } 

        private Int32? _Identifikator; 
        [DataMember] 
        public virtual Int32? Identifikator  
        {   
            get   
            {  
                return _Identifikator; 
            } 
            set 
            {
                if (value != _Identifikator) 
                {
                    _Identifikator = value; 
                    OnPropertyChanged("Identifikator"); 
                } 
            } 
        } 

        private Guid? _TransakciaZruseneId; 
        [DataMember] 
        public virtual Guid? TransakciaZruseneId  
        {   
            get   
            {  
                return _TransakciaZruseneId; 
            } 
            set 
            {
                if (value != _TransakciaZruseneId) 
                {
                    _TransakciaZruseneId = value; 
                    OnPropertyChanged("TransakciaZruseneId"); 
                } 
            } 
        } 


        public virtual string GetTableName  
        {   
            get   
            {  
                return "FO.UDAJE_POBYTU"; 
            } 
        } 

	}
}
