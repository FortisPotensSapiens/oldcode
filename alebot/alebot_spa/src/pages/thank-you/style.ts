export const styles = {
    container:{
        width:'100%',
        height: '77vh',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
    },
    content:{
        display: 'flex',
        flexDirection:'column',
        justifyContent: 'center',
        alignItems: 'center',
        width: '100%',
        height: '100%',
        padding: '20px',
        borderRadius: '8px',
        textAlign: 'center',
        background: 'rgba(255, 255, 255, 0.9)',
        boxShadow: '0 0 20px rgba(0, 0, 0, 0.2)',
        '& h1': {
            color: '#008080'
        },
        '& p':{
            margin: '10px 0',
        }
    },
    button:{
        backgroundColor:'#C39F54',
        color: '#fff',
        padding: '10px 40px',
        marginTop:"20px",
        '&:hover':{
            backgroundColor:'#7e6728',
        }
    }
}