export const styles = {
    box:{
        width:'100%',
        borderRadius: '20px',
        background: '#FFF',
        minHeight:'382px',
        padding:'22px 37px',
        marginRight:'15px',
        '@media only screen and (max-width: 640px)': {
            marginTop: '30px',
            overflow: 'auto'
        }
    },
    noData:{
        color: '#BFBFBF',
        fontSize: '16px',
        fontWeight: '400',
        marginTop:3,
    },
    dateShower:{
        borderRadius: '5px',
        border: '1px solid #EEE',
        background: '#FFF',
        color: '#1E1E1E',
        fontSize: '10px',
        fontWeight: '600',
        lineHeight: 'normal',
        padding: '9px 13px',
        display:'flex',
        alignItems:'center',
        position: 'relative',
        '& .rdrDateDisplayWrapper':{
            display: 'none'
        }
    },
    datepickerWrapper:{
        position:'absolute',
        top:'40px',
        right:'0',
        backgroundColor:'red !important',
        boxShadow:'1px 1px 7px 1px lightgray',
        borderRadius:'7px',
        overflow:'hidden',
    },
    table:{
        minWidth: 650,
        '@media only screen and (max-width: 640px)': {
            minWidth:'unset',
        }
    },
    stateCreated:{
        color:'#C39F54',
        fontWeight:'bold'
    },
    stateCanceled:{
        color:'red',
        fontWeight:'bold'
    },
    stateCompleted:{
        color:'#2F5549',
        fontWeight:'bold'
    }
}