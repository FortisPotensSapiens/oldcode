export const styles = {
    container:{
        display:'flex',
        flexDirection:'column',
        width: '100%',
        maxWidth: '1200px',
    },
    box:{
        display: 'flex',
        width:'100%',
        '@media only screen and (max-width: 1200px)': {
            flexDirection:'column'
        },
    },
    balance:{
        width:'100%',
        maxWidth:'636px',
        minWidth:'636px',
        minHeight:'382px',
        borderRadius: '20px',
        background: 'linear-gradient(111deg, #C1870D -4.47%, #FFF 17.88%, #C39F54 45.33%, #FFF 70.21%, #C08916 98.34%)',
        '@media only screen and (max-width: 1200px)': {
           maxWidth:'unset',
           minWidth:'unset',
        },
    },
    titleBox:{
        display:'flex',
        justifyContent:'space-between',
        alignItems:'center',
        padding:'22px 37px',
    },
    balanceTitle:{
        color: '#1E1E1E',
        fontSize: '20px',
        fontWeight: '600',
        lineHeight: 'normal',

    },
    currencySymbol:{
        borderRadius: '5px',
        background: '#2F5549',
        display:'flex',
        width:'32px',
        height:'39px',
        color: '#fff',
        justifyContent: 'center',
        alignItems: 'center',
    },
    moneyAmount:{
        color: '#404040',
        fontSize: '64px',
        fontWeight: '600',
        lineHeight: 'normal',
        marginLeft: '37px',
    },
    smallPriceTitle:{
        color: '#fff',
        fontSize: '10px',
        fontWeight: '400',
        lineHeight: 'normal',
    },
    smallPrice:{
        color: '#2F5549',
        fontSize: '20px',
        fontWeight: '600',
        lineHeight: 'normal',
    },
    smallPriceWrapper:{
        display:'flex',
        flexDirection:'column',
        justifyContent:'space-between',
        marginLeft:'12px',
    },
    withdrawButton:{
        display:'flex',
        width: '100%',
        height: '58px',
        borderRadius: '5px',
        background: '#2F5549',
        color: '#FFF',
        fontSize: '16px',
        fontWeight: '500',
        lineHeight: 'normal',
        marginRight:'15px',
        '@media only screen and (max-width: 510px)': {
            flexDirection:'column',
            height:'unset',
            padding:'15px 0',
            fontSize:'12px',
        },
        '&:hover':{
            background: '#25463c',
            color: '#FFF',
        }
    },
    topUpButton:{
        display:'flex',
        width: '100%',
        height: '58px',
        borderRadius: '5px',
        background: '#404040',
        color: '#FFF',
        fontSize: '16px',
        fontWeight: '500',
        lineHeight: 'normal',
        marginRight:'15px',
        whiteSpace:'nowrap',
        '@media only screen and (max-width: 510px)': {
            flexDirection:'column',
            height:'unset',
            padding:'15px 0',
            fontSize:'12px'
        },
        '&:hover':{
            background: '#404040',
            color: '#FFF',
        }
    },
    info:{
        display:'flex',
        width:'100%',
        minWidth:'282px',
        borderRadius: '20px',
        background: '#FFF',
        marginLeft:'15px',
        padding: '22px 37px',
        flexDirection:'column',
        '@media only screen and (max-width: 1200px)': {
           marginLeft:0,
            marginTop:'20px',
        },
    },
    infoValue:{
        display:'flex',
        alignItems:'center',
        marginLeft:'-5px',
        marginTop: '22px',
        '& img':{
            marginRight:'14px',
        },
        '& a':{
            color:'#003397 !important',
            display:'flex',
        }
    },
    infoValueTitle:{
        color: '#404040',
        fontSize: '14px',
        fontWeight: '700',
        lineHeight: 'normal',
    },
    infoValueText:{
        color: '#404040',
        fontSize: '14px',
        fontStyle: 'normal',
        fontWeight: '400',
        marginTop:'6px',
    }
}