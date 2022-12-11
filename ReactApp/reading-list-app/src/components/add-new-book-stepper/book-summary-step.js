import React, {useEffect} from "react";
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Typography from '@mui/material/Typography';

import AuthorService from "../../services/author-service";
import CategoryService from "../../services/category-service";

 

export default function BookSummaryStep(props) {
    const [author, setAuthor] = React.useState('');
    const [category, setCategory] = React.useState('');  
    
    const _authorService = new AuthorService();
    const _categoryService = new CategoryService();

    const statuses = ['Scheduled', 'In progress', 'Completed', 'Skipped'];
    const priority = ['Never', 'Low', 'Medium', 'High', 'Urgent', 'RightNow'];  

    useEffect(() => {
        async function getAuthor() {   
          const author = await _authorService.getAuthorByIdFromApi(props.value.authorId);
          setAuthor(author.fullName);
        };

        async function getCategory() {   
            const category = await _categoryService.getCategoryByIdFromApi(props.value.categoryId);
            setCategory(category.name);
          };

        if (author === '') {
          getAuthor();
        }

        if (category === '') {
            getCategory();
        }
    });    

  return (
    <Card sx={{ minWidth: 275, margin: 2  }}>
      <CardContent>
        <Typography sx={{ fontSize: 14 }} color="text.secondary" gutterBottom>
          Book summary
        </Typography>
        <Typography variant="h5" component="div">
          {props.value.title}
        </Typography>
        <Typography sx={{ mb: 1.5 }} color="text.secondary">
          by {author}
        </Typography>
        <Typography variant="body2">
          <b>category:</b> {category}.
          <br />
          <br />
          <b>priority:</b> {priority[props.value.priority]}.
          <br />
          <b>status:</b> {statuses[props.value.status]}.
        </Typography>
      </CardContent>
    </Card>
  );
}