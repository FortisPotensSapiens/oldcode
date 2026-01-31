export const styles = {
    box:{
        borderRadius: '20px',
        background: '#404040',
        marginLeft:'15px',
        overflow:'hidden',
        '& .MuiSvgIcon-root':{
            color: '#fff',
        },
        '@media only screen and (max-width: 1200px)': {
            marginLeft:0,
            marginTop:'20px',
        },
        '@media only screen and (max-width: 500px)': {
            marginBottom:'100px'
        },
    },
    sectionTitle:{
        color: '#FFF',
        fontSize: '20px',
        fontWeight: '600',
        lineHeight: 'normal',
    },
    infoText:{
        display: 'flex',
        alignItems:'center',
        color: '#FFF',
        fontSize: '16px',
        fontWeight: '500',
        lineHeight: 'normal',
        '& span':{
            display:'flex',
            width:'1px',
            height:'45px',
            background:'#666',
            margin:'0 50px',
        },
        '& b':{
            marginLeft: '10px',
        }
    },
    iText:{
        color: 'rgba(255, 255, 255, 0.42)',
        fontSize: '16px',
        fontWeight: '400',
        lineHeight: 'normal',
        width: '125px',
        marginRight:'6px',
    },
    iIcon:{
        position:'relative',
        cursor:'pointer',
        '&:hover .popOver':{
            display:'flex',
        },
         '& .popOver':{
             borderRadius: '5px',
             background: '#2F5549',
             boxShadow: '1px 4px 16px 0px rgba(0, 0, 0, 0.25)',
             width:'165px',
             color: '#FFF',
             textAlign: 'center',
             fontSize: '10px',
             fontWeight: '500',
             lineHeight: 'normal',
             padding:'10px 14px',
             position: 'absolute',
             cursor: 'pointer',
             top:'0',
             left:'30px',
             display:'none',
         }
    }
}