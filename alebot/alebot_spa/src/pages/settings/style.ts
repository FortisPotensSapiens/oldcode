import telegramIcon from '../../assets/svg/telegramIcon.png'
export const styles = {
    box:{
        borderRadius: '20px',
        background: '#FFF',
        width:'100%',
        minHeight:'378px',
        mt:4,
        padding:'10px 40px'
    },
    title:{
        color: '#404040',
        fontSize: '20px',
        fontWeight: '600',
        lineHeight: 'normal',
        display:'flex',
        alignItems:'center',
        '& span':{
            color:'#058c05',
            fontSize:'14px',
            marginLeft:'15px',
        },
        '& span.error':{
            color:'red',
            fontSize:'14px',
            marginLeft:'15px',
        }
    },
    inputContainer:{
        display: 'grid',
        gridTemplateColumns:'repeat(2,1fr)',
        gap:'15px',
        '@media only screen and (max-width: 1045px)': {
            display:'flex',
            flexDirection:'column',
        },
    },
    inputWrapper:{
        width: '287px',
        height:'50px',
        border: '1px solid #EEE',
        display:'flex',
        flexDirection:'column',
        padding: '8px 15px',
        position:'relative',
        '@media only screen and (max-width: 1045px)': {
           width:'100%',
        },
        '&.error':{
            border: '1px solid red',
            '& label':{
                color:'red'
            }
        },
        '& label':{
            color: '#1E1E1E',
            fontSize: '10px',
            fontWeight: '700',
            lineHeight: 'normal',
        },
        '& input':{
            border: 'none !important',
            outline: 'none !important',
            color: '#1E1E1E',
            fontSize: '14px',
            fontWeight: '400',
            marginTop:'3px',
            width:'90%',
        },
    },
    telegramIcon:{
        position: 'absolute',
        right:'13px',
        top:'14px'
    },
    submitButton:{
        display: 'flex',
        width: '182px',
        height: '35px',
        padding: '15px 39px',
        justifyContent: 'center',
        alignItems: 'center',
        borderRadius: '4px',
        background: '#2F5549',
        color: '#FFF',
        fontSize: '14px',
        fontWeight: 500,
        marginTop: '100px',
        '&:hover':{
            background: '#2F5549',
            color: '#FFF',
        }
    },
    imageUploader:{
        width: '115px',
        height: '115px',
        border: '1px solid #EEE',
        display:'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'center',
        marginLeft:'27px',
        '& p': {
            color: '#1E1E1E',
            fontSize: '10px',
            fontWeight: '700',
            lineHeight: 'normal',
            maxWidth:'50%',
            marginTop:'10px',
            textAlign:'center',
        },
        '& span': {
            color: '#BFBFBF',
            fontSize: '10px',
            fontWeight: '500',
            marginTop:'4px',
            textAlign:'center',
        }
    },
    avatar:{
        width: '115px',
        height: '115px',
        marginLeft:'27px',
        objectFit:'cover',
        borderRadius:'100%'
    }
}