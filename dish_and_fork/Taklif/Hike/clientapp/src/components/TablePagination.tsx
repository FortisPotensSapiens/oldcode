import {
  Box,
  styled,
  Table,
  TablePagination as MuiTablePagination,
  TablePaginationProps as MuiTablePaginationProps,
  TableRow,
} from '@mui/material';
import { FC } from 'react';

const TABLE_PAGINATION_ROW_COUNT_ITEMS = [5, 10, 25, 50, 100];

const StyledContainer = styled(Box)(({ theme }) => ({
  width: '100%',
  position: 'absolute',
  bottom: 0,
  backgroundColor: 'rgba(255, 255, 255, 1)',
  [theme.breakpoints.down('sm')]: {
    position: 'fixed',
    bottom: '60px',
    backgroundColor: 'rgba(255, 255, 255, 0.9)',
  },
}));

type TablePaginationProps = Omit<MuiTablePaginationProps, 'count'> & Partial<Pick<MuiTablePaginationProps, 'count'>>;

const TablePagination: FC<TablePaginationProps> = ({
  count,
  rowsPerPageOptions = TABLE_PAGINATION_ROW_COUNT_ITEMS,
  ...props
}) => (
  <StyledContainer>
    <Table>
      <TableRow>
        <MuiTablePagination
          {...props}
          count={count ?? 0}
          rowsPerPageOptions={rowsPerPageOptions}
          labelRowsPerPage="Строк в странице"
        />
      </TableRow>
    </Table>
  </StyledContainer>
);

export { TABLE_PAGINATION_ROW_COUNT_ITEMS, TablePagination };
export type { TablePaginationProps };
