import { Link as MuiLink, LinkProps as MuiLinkProps } from '@mui/material';
import { FC } from 'react';
import { Link as RouterLink, LinkProps as RouterLinkProps } from 'react-router-dom';

type LinkProps = MuiLinkProps & RouterLinkProps;

const Link: FC<LinkProps> = (props) => <MuiLink {...props} component={RouterLink} />;

export { Link };
