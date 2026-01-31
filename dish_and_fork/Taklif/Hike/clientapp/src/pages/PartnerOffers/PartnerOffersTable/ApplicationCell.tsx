import { Link } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';

import { generatePartnerNewAppPath } from '~/routing';

import { ColumnProps } from '../types';

const ApplicationCell = ({ row }: ColumnProps) =>
  row.applictaion ? (
    <Link
      color="primary.main"
      component={RouterLink}
      to={generatePartnerNewAppPath(row.applictaion.id)}
      underline="hover"
    >
      № {row.applictaion.number} {row.applictaion.title}
    </Link>
  ) : null;

export { ApplicationCell };
