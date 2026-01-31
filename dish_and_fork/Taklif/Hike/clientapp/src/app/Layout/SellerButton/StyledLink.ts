import { styled } from '@mui/material';
import { Link } from 'react-router-dom';

const StyledLink = styled(Link, { name: 'StyledLink' })(() => ({ whiteSpace: 'nowrap' }));

export { StyledLink };
