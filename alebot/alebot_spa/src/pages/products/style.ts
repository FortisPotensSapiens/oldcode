export const styles = {
    container:{
        display:'grid',
        gridTemplateColumns:'repeat(4,1fr)',
        gridGap:'15px',
        maxWidth:'unset !important',
        paddingLeft:'0 !important',
        paddingRight:'0 !important',
        '@media only screen and (max-width: 1800px)': {
            gridTemplateColumns:'repeat(3,1fr)',
        },
        '@media only screen and (max-width: 1140px)': {
            gridTemplateColumns:'repeat(2,1fr)',
        },
        '@media only screen and (max-width: 800px)': {
            gridTemplateColumns:'repeat(1,1fr)',
        }
    }
}