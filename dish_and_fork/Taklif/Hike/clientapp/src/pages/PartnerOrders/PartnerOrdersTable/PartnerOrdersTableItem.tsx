import { styled } from '@mui/material';
import TableCell from '@mui/material/TableCell';
import TableRow from '@mui/material/TableRow';

import { Column, Row } from './types';

const StyledRow = styled(TableRow)({
  cursor: 'pointer',
});

const PartnerOrdersTableItem = ({
  columns,
  onRowClick,
  row,
}: {
  row: Row;
  columns: Column[];
  onRowClick: (orderId: string) => void;
}): JSX.Element => {
  const onRowClickCallback = () => {
    onRowClick(row.id);
  };

  return (
    <StyledRow hover onClick={onRowClickCallback} tabIndex={-1}>
      {columns.map((column) => {
        const value = (row as any)[column.id] ?? '';

        return (
          <TableCell key={column.id}>
            {column.format ? column.format(row) : value} {column.after ? column.after(row) : undefined}
          </TableCell>
        );
      })}
    </StyledRow>
  );
};

export default PartnerOrdersTableItem;
