import React from "react";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Unstable_Grid2";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import FormControl from "@mui/material/FormControl";
import Select from "@mui/material/Select";
import Typography from "@mui/material/Typography";
import Paper from "@mui/material/Paper";
import Rating from "@mui/material/Rating";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import IconButton from "@mui/material/IconButton";
import Button from "@mui/material/Button";
import { alpha } from "@mui/material/styles";
import Tooltip from "@mui/material/Tooltip";
import Toolbar from "@mui/material/Toolbar";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
// Import custom components
import CategoryStep from "../add-new-book-stepper/category-step";
// Import services
import CategoryService from "../../services/category-service";
// Import data transfer objects and utils
import CategoryDto from "../../dto/category-dto";
import Priorities from "../../utils/priorities-list";
import Statuses from "../../utils/statuses-list";

const _categoryService = new CategoryService();

export default function EditBookForm(props) {
  const {
    tableHeight,
    model,
    onEditBookBackClick,
    onPriorityChange,
    onSaveButtonClick,
    onStatusChange,
    onCategorySelect,
  } = props;

  const statuses = Statuses.slice();

  return (
    <Box sx={{ width: "100%" }}>
      <Paper
        sx={{
          width: "100%",
          minHeight: tableHeight - 54 /*54 - offset chosen empirically*/,
          mb: 2,
        }}
      >
        <Toolbar
          sx={{
            pl: { sm: 2 },
            pr: { xs: 1, sm: 1 },
            marginBottom: 2,
            ...{
              bgcolor: (theme) => alpha(theme.palette.primary.main),
            },
          }}
        >
          <Typography
            sx={{ flex: "1 1 100%" }}
            variant="h6"
            id="tableTitle"
            component="div"
          >
            Edit book note
          </Typography>
          <Tooltip title="Back">
            <IconButton onClick={onEditBookBackClick}>
              <ArrowForwardIcon />
            </IconButton>
          </Tooltip>
        </Toolbar>

        <Card sx={{ minWidth: 275, margin: 2 }}>
          <CardContent>
            <Typography variant="h5" component="div">
              "{model.title}"
            </Typography>
            <Typography sx={{ mb: 1.5 }} color="text.secondary">
              by {model.author}
            </Typography>

            <Grid container spacing={2}>
              <Grid xs={12}>
                <CategoryStep
                  categoryId={model.categoryId}
                  onSelect={(id) => onCategorySelect(id)}
                />
              </Grid>
              <Grid xs={6}>
                <Paper sx={{ minHeight: 130 }} variant="outlined">
                  <Typography sx={{ padding: 0 }} variant="body2">
                    <h3>Set new priority</h3>
                    <p>{Priorities[model.priority]}.</p>
                  </Typography>
                  <Rating
                    name="simple-controlled"
                    value={model.priority === undefined ? 0 : model.priority}
                    size="large"
                    onChange={(event) => onPriorityChange(event)}
                  />
                </Paper>
              </Grid>
              <Grid xs={6}>
                <Paper sx={{ minHeight: 130 }} variant="outlined">
                  <Typography sx={{ padding: 0 }} variant="body2">
                    <h3>Set new status</h3>
                  </Typography>
                  <FormControl sx={{ m: 2, minWidth: 200 }} size="small">
                    <InputLabel id="demo-simple-select-label">
                      Status
                    </InputLabel>
                    <Select
                      labelId="demo-simple-select-label"
                      id="demo-simple-select"
                      value={model.status == undefined ? 0 : model.status}
                      label="Category"
                      onChange={onStatusChange}
                    >
                      {statuses.map((item) => (
                        <MenuItem
                          key={statuses.indexOf(item)}
                          value={statuses.indexOf(item)}
                        >
                          {item}
                        </MenuItem>
                      ))}
                    </Select>
                  </FormControl>
                </Paper>
              </Grid>
            </Grid>
          </CardContent>
        </Card>
        <Box sx={{ flex: "1 1 auto" }} />
        <Button
          disabled={model.categoryId === undefined}
          onClick={onSaveButtonClick}
        >
          Save
        </Button>
      </Paper>
    </Box>
  );
}
