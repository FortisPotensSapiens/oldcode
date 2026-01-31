export const styles = {
    box: {
        width: '100%',
        borderRadius: '20px',
        background: '#FFF',
        padding: '0 32px',
        maxWidth: '1440px',
        marginTop: '50px',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
    },
    photo: {
        width: '200px',
        height: '200px',
        objectFit: 'cover',
        borderRadius: '100%',
        border: '1px solid #F6F8FF',
        marginBottom:'4px',
        margin:'auto',
    },
    name: {
        color: '#BE8B20',
        fontSize: '24px',
        fontWeight: '700',
        lineHeight: 'normal',
        textAlign: 'center',

        '@media only screen and (max-width:640px)': {
            fontSize:'14px'
        },
    },
    licenseKey: {
        color:'#000',
        fontWeight:'600',
        fontSize:'14px',
        textAlign:'left',
        width:'100%',
        '& span': {
            color:'#C39F54',
            textDecoration:'underLine',
            position: 'relative',
            svg:{
                position:'absolute',
                right: '-5px',
                top: '3px',
                width: '26px',
                height: '26px',
            }
        }
    },
    shortDescription: {
        color: '#000',
        fontSize: '10px',
        fontWeight: '400',
        lineHeight: 'normal',
        marginTop: '14px',
        textAlign: 'center',
    },
    price: {
        color: '#BE8B20',
        fontSize: '20px',
        fontWeight: '700',
        lineHeight: 'normal',
        marginTop: '25px',
        textAlign: 'center',
    },
    button: {
        borderRadius: '5px',
        background: '#BE8B20',
        color: '#FFF',
        fontSize: '14px',
        fontWeight: '700',
        lineHeight: 'normal',
        width: '167px',
        height: '50px',
        marginTop: '25px',
        '&:hover': {
            background: '#a28342',
            color: '#FFF',
        }
    },
    mobileTitles:{
        display: 'flex',
        flexDirection: 'column',
        borderBottom:'1px solid #EEEEEE',
        paddingBottom:'14px',
        marginTop:'10px',
        '& b':{
            fontSize:'10px',
        },
        '& span':{
            fontSize:'12px',
            marginTop:"6px",
        }
    }
}