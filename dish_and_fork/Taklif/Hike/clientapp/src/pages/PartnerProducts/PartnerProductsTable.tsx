import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';

import { LoadingSpinner } from '~/components/LoadingSpinner';
import { getCurrencySymbol } from '~/utils/getCurrencySymbol';

import { RenderActionsCell } from './RenderActionsCell';
import { RenderPhotoCell } from './RenderPhotoCell';
import { RenderStateCell } from './RenderStateCell';
import { Row } from './utils';

interface Column {
  after?: (item: Row) => unknown;
  format?: (item: Row) => unknown;
  id: 'id' | 'photo' | 'title' | 'price' | 'state' | 'createDate' | 'availableQuantity';
  label: string;
}

const useColumns = (onTogglePublish: (rowId: string, state: boolean) => void) => {
  const columns: Column[] = [
    {
      format: (item: Row) => {
        return <RenderPhotoCell row={item} />;
      },
      id: 'photo',
      label: 'Фото',
    },

    {
      id: 'title',
      label: 'Наименование товара',
    },

    {
      format: (item: Row) => {
        return (
          <Box>
            {item.price} {getCurrencySymbol('Rub')}
          </Box>
        );
      },
      id: 'price',
      label: 'Цена',
    },
    {
      id: 'createDate',
      label: 'Дата добавления',
    },
    {
      id: 'availableQuantity',
      label: 'Готово к отправке',
    },
    {
      format: (item: Row) => {
        return <RenderStateCell row={item} onTogglePublish={onTogglePublish} />;
      },
      id: 'state',
      label: 'Статус',
    },
  ];

  return columns;
};

const PartnerProductsTable = ({
  footer,
  isLoading,
  rows,
  onDelete,
  onTogglePublish,
}: {
  rows: Row[];
  footer: JSX.Element;
  isLoading: boolean;
  onDelete: (rowId: string) => void;
  onTogglePublish: (rowId: string, state: boolean) => void;
}) => {
  const columns = useColumns(onTogglePublish);

  return (
    <>
      <TableContainer component={Paper}>
        <Table sx={{ minWidth: 500 }}>
          <TableHead>
            <TableRow>
              {columns.map((column) => (
                <TableCell key={column.id}>{column.label}</TableCell>
              ))}
              <TableCell>Действия</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {isLoading ? (
              <TableRow>
                <TableCell colSpan={6}>
                  <LoadingSpinner />
                </TableCell>
              </TableRow>
            ) : undefined}
            {rows?.map((row) => {
              return (
                <TableRow key={row.id} hover role="checkbox" tabIndex={-1}>
                  {columns.map((column) => {
                    const value = row[column.id];

                    return (
                      <TableCell key={column.id}>
                        {column.format ? column.format(row) : value} {column.after ? column.after(row) : undefined}
                      </TableCell>
                    );
                  })}

                  <TableCell width={150}>
                    <RenderActionsCell row={row} onDelete={onDelete} />
                  </TableCell>
                </TableRow>
              );
            })}
          </TableBody>
        </Table>
      </TableContainer>

      {footer}
    </>
  );
};

export { PartnerProductsTable };
