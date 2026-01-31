import { Grid } from '@mui/material';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import { TablePagination } from '~/components/TablePagination';
import { generateIndividualOrderPath } from '~/routing';
import { ApplicationReadModel, ApplicationReadModelPageResultModel } from '~/types';

import { IndividualOrdersTableItem } from './IndividualOrdersTableItem';

const ROW_COUNT_ITEMS = [5, 10, 25, 50];

const IndividualOrdersTable = ({
  data,
  handleChangePage,
  handleChangeRowsPerPage,
  page,
  rowsPerPage,
  onDelete,
}: {
  handleChangePage: (_event: unknown, newPage: number) => void;
  data: ApplicationReadModelPageResultModel | undefined;
  page: number;
  rowsPerPage: number;
  handleChangeRowsPerPage: (event: React.ChangeEvent<HTMLInputElement>) => void;
  onDelete: (rowId: string) => void;
}) => {
  const navigate = useNavigate();
  const [rows, setRows] = useState<ApplicationReadModel[]>([]);

  useEffect(() => {
    setRows(data?.items ?? []);
  }, [data]);

  const onClick = (rowId: string) => {
    navigate(generateIndividualOrderPath(rowId));
  };

  return (
    <Grid alignContent="space-between" container direction="row" height="100%" width="100%">
      <Grid flexGrow={1} item width="100%" marginBottom={8}>
        <Grid container spacing={{ sm: 3, xl: 4, xs: 2 }}>
          {rows.map((row) => (
            <Grid key={row.id} item md={4} sm={6} xs={12}>
              <IndividualOrdersTableItem
                isDeletable={!row.selectedOrderId}
                hasOfferLink={!!row.selectedOfferId}
                onClick={onClick}
                onDelete={onDelete}
                row={row}
                hasOrderLink={!!row.selectedOrderId}
              />
            </Grid>
          ))}
        </Grid>
      </Grid>

      <TablePagination
        colSpan={5}
        count={data?.totalCount ?? 0}
        labelRowsPerPage="Строк в странице"
        onPageChange={handleChangePage}
        onRowsPerPageChange={handleChangeRowsPerPage}
        page={page}
        rowsPerPage={rowsPerPage}
        rowsPerPageOptions={ROW_COUNT_ITEMS}
      />
    </Grid>
  );
};

export { IndividualOrdersTable, ROW_COUNT_ITEMS };
