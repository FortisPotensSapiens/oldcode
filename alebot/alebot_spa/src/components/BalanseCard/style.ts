export const styles = {
    box:{
        width:'421px',
        minWidth:'421px',
        height:'381px',
        backgroundColor:'#fff',
        borderRadius:'20px',
        padding:'22px 37px',
        '@media only screen and (max-width: 640px)': {
            width:'100%',
            minWidth:'unset',
            padding:'22px 6px',
        }
    },
    price:{
        color: '#404040',
        fontSize: '64px',
        fontWeight: '600',
        lineHeight: 'normal',
        marginTop:'14px',
    },
    smallPriceTitle:{
        color: '#1E1E1E',
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
        width: '166px',
        height: '58px',
        borderRadius: '5px',
        background: '#BFBFBF',
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
            background: '#BFBFBF',
            color: '#FFF',
        }
    },
    topUpButton:{
        width: '166px',
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
    applyButton:{
        width: '166px',
        height: '58px',
        borderRadius: '5px',
        background: '#404040',
        color: '#FFF',
        fontSize: '16px',
        fontWeight: '500',
        lineHeight: 'normal',
        marginRight:'15px',
        whiteSpace:'nowrap',
        minWidth: '200px',
        gap:'10px',
        '&:hover':{
            background: '#404040',
            color: '#FFF',
        }
    }
}