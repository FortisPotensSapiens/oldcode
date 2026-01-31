export const styles = {
    box:{
        width:'100%',
        borderRadius: '20px',
        background: '#2F5549',
        minHeight:'455px',
        display:'flex',
        flexDirection:'column',
        justifyContent: 'center',
    },
    structure:{
        color: '#FFF',
        fontSize: '20px',
        fontWeight: '700',
        lineHeight: 'normal',
        marginTop:'22px',
        marginLeft:'35px',
        '@media only screen and (max-width: 1060px)': {
            marginBottom:'20px !important',
        },
    },
    noDataWrapper:{
        display:'flex',
        flexDirection:"column",
        width: '100%',
        alignItems:'center',
        marginTop: '5px',
    },
    noDataTitle:{
        color: '#FFF',
        textAlign: 'center',
        fontSize: '20px',
        fontWeight: '400',
        lineHeight: 'normal',
        maxWidth:'355px',
        '@media only screen and (max-width: 640px)': {
            maxWidth:'calc(100% - 32px)',
            fontSize:'12px',
        },
    },
    noDataImage:{
        maxWidth:'281px',
        height:'fit-content',
        objectFit:'contain',
        marginTop:'28px',
        '@media only screen and (max-width: 640px)': {
            maxWidth:'calc(100% - 32px)',
        },
    },
    reload:{
        color: 'rgba(255, 255, 255, 0.42)',
        fontSize: '12px',
        fontWeight: '600',
        marginLeft: '10px',
    },
    structureWrapper:{
        display: 'flex',
        justifyContent:'space-between',
        paddingTop:'22px',
        paddingRight: '35px',
        paddingLeft: '35px',
        paddingBottom:'35px',
        alignItems: 'center',
        position: 'relative',
        '@media only screen and (max-width: 1060px)': {
            flexDirection:'column',
        },
        '& .line':{
            width:'calc(100% - 40px)',
            position:'absolute',
            left:'35px',
            bottom:'0',
        }
    },
    detailsBox:{
        display:'flex',
        flexDirection: 'column',
        paddingRight:'70px',
        '@media only screen and (max-width: 1060px)': {
            paddingRight:'30px',
        },
        '& span':{
            color: '#FFF',
            fontSize: '20px',
            fontWeight: '800',
            lineHeight: 'normal',
        },
        '& p':{
            color: '#FFF',
            fontSize: '12px',
            fontWeight: '400',
            lineHeight: 'normal',
        }
    },
    catalog:{
        display:'grid',
        gridTemplateColumns:'repeat(4,1fr)',
        gap:'30px',
        padding:'23px 35px',
        '@media only screen and (max-width: 1260px)': {
            gridTemplateColumns:'repeat(3,1fr)',
        },
        '@media only screen and (max-width: 1000px)': {
            gridTemplateColumns:'repeat(2,1fr)',
        },
        '@media only screen and (max-width: 830px)': {
            gridTemplateColumns:'repeat(1,1fr)',
        }
    }
}