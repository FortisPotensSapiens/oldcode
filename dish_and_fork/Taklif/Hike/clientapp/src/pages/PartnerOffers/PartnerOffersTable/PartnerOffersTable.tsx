import Paper from '@mui/material/Paper/Paper';
import Table from '@mui/material/Table/Table';
import TableBody from '@mui/material/TableBody/TableBody';
import TableCell from '@mui/material/TableCell/TableCell';
import TableContainer from '@mui/material/TableContainer/TableContainer';
import TableHead from '@mui/material/TableHead/TableHead';
import TableRow from '@mui/material/TableRow/TableRow';

import { OfferSellerReadModel } from '~/types/swagger';

import { useColumns } from './useColumns';

const PartnerOffersTable = ({
  rows,
  onDelete,
}: {
  rows: OfferSellerReadModel[];
  onDelete: (offerId: string) => void;
}) => {
  const columns = useColumns();

  return (
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 500 }}>
        <TableHead>
          <TableRow>
            {columns.map((column) => (
              <TableCell key={column.field}>{column.headerName}</TableCell>
            ))}
          </TableRow>
        </TableHead>
        <TableBody>
          {rows?.map((row: any) => {
            return (
              <TableRow key={row.id} hover role="checkbox" tabIndex={-1}>
                {columns.map((column) => {
                  const value = row[column.field];
                  const Component = column.renderCell;

                  return (
                    <TableCell key={column.field}>
                      {Component ? (
                        <Component row={row} onDelete={onDelete} />
                      ) : (
                        column.valueFormatter?.({
                          value,
                        })
                      )}
                    </TableCell>
                  );
                })}
              </TableRow>
            );
          })}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export { PartnerOffersTable };
