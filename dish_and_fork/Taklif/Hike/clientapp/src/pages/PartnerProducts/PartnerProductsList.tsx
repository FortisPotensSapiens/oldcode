import { Box, Grid, Paper, styled, Typography } from '@mui/material';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import { Fragment } from 'react';

import { LoadingSpinner } from '~/components/LoadingSpinner';
import { getCurrencySymbol } from '~/utils/getCurrencySymbol';

import { RenderActionsCell } from './RenderActionsCell/RenderActionsCell';
import { RenderStateCell } from './RenderStateCell';
import { Row } from './utils';

const StyledPaper = styled(Paper)(({ theme }) => ({ marginBottom: theme.spacing(4) }));

const PartnerProductsList = ({
  footer,
  isLoading,
  rows,
  onDelete,
  onTogglePublish,
}: {
  rows: Row[] | undefined;
  footer: JSX.Element;
  isLoading: boolean;
  onDelete: (rowId: string) => void;
  onTogglePublish: (rowId: string, state: boolean) => void;
}): JSX.Element => {
  return (
    <>
      <List>
        {isLoading ? <LoadingSpinner /> : undefined}

        {rows?.map((row) => {
          return (
            <Fragment key={row.id}>
              <ListItem>
                <StyledPaper>
                  <Grid container marginBottom={2}>
                    <Grid item>
                      <Box paddingBottom={2} width="100%">
                        <Box component="img" src={row.photo ?? ''} width="100%" />
                      </Box>
                    </Grid>
                    <Grid item padding={2} width="100%">
                      <Grid container justifyContent="space-between">
                        <Grid item>
                          <Typography paddingBottom={1} variant="h5">
                            {row.title}
                          </Typography>
                        </Grid>
                      </Grid>
                      <Grid container justifyContent="space-between">
                        <Grid item>
                          <Typography variant="h6">
                            {row.price} {getCurrencySymbol('Rub')}
                          </Typography>
                        </Grid>
                        <Grid item>
                          <Box style={{ opacity: 0.4 }} textAlign="right">
                            <Typography variant="body1">{row.createDate}</Typography>
                          </Box>
                        </Grid>
                      </Grid>
                    </Grid>
                    <Grid item xs={12}>
                      <Box paddingBottom={1} textAlign="center">
                        <Typography variant="body2">
                          <RenderStateCell row={row} onTogglePublish={onTogglePublish} />
                        </Typography>
                      </Box>
                    </Grid>
                    <Grid item xs={12}>
                      <Box paddingBottom={1} textAlign="center">
                        <Typography variant="body1">Готово к отправке: {row.availableQuantity}</Typography>
                      </Box>
                    </Grid>
                    <Grid item paddingLeft={4}>
                      <RenderActionsCell row={row} onDelete={onDelete} />
                    </Grid>
                  </Grid>
                </StyledPaper>
              </ListItem>
            </Fragment>
          );
        })}
      </List>
      {footer}
    </>
  );
};
export { PartnerProductsList };
