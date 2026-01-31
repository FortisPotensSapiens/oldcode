export const styles = {
    box:{
        width:'100%',
        minHeight:'458px',
        backgroundColor:'#fff',
        borderRadius:'20px',
        marginTop:'70px',
        display:'flex',
        padding:'0 32px',
        paddingBottom:'32px',
        '@media only screen and (max-width: 960px)': {
            flexDirection:' column',
        }
    },
    title:{
        color: '#1E1E1E',
        fontSize: '24px',
        fontWeight: '700',
        lineHeight: 'normal',
        marginTop: '22px',
        textAlign:'center',
    },
    price:{
        color: '#BE8B20',
        fontSize: '20px',
        fontWeight: '700',
        lineHeight: 'normal',
        marginTop:'20px',
        textAlign:'center',
    },
    button:{
        width: '166px',
        height: '58px',
        borderRadius: '5px',
        background: '#404040',
        marginTop:'40px',
        color: '#FFF',
        fontSize: '16px',
        fontWeight: '500',
        lineHeight: 'normal',
        '&:hover':{
            color: '#FFF',
            background: '#2f2e2e',
        }
    },
    photo:{
        width: '200px',
        height:'200px',
        objectFit:'cover',
        position:'relative',
        bottom:'40px',
        borderRadius: '100%',
        border: '1px solid #F6F8FF',
    },
    imageWrapper:{
        minWidth:'247px',
        display: 'flex',
        flexDirection:'column',
        alignItems:'center',
        marginRight:'30px',
    },
    des:{
        color: '#000',
        fontSize: '16px',
        fontWeight: '700',
        lineHeight: 'normal',
        marginTop:'30px',
    }
}