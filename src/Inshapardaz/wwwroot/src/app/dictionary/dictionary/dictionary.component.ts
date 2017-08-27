import { Component } from '@angular/core';
import { Router, ActivatedRoute, NavigationExtras } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { FormsModule , FormBuilder } from '@angular/forms';
import { Observable }  from 'rxjs/Observable';

import {Languages} from '../../../models/language';
import { DictionaryService } from '../../../services/dictionary.service';
import { Dictionary } from '../../../models/dictionary';
import { Word } from '../../../models/word';
import { WordPage } from '../../../models/WordPage';
import { DictionaryIndex } from '../../../models/Dictionary';
export class Index{
    title : string;
    link : string;
}
@Component({
    selector: 'dictionary',
    templateUrl: './dictionary.component.html'
})

export class DictionaryComponent {
    private sub: Subscription;
    isInSearch : boolean = false;
    isLoading : boolean = false;
    errorMessage: string;
    id : number;
    dictionary : Dictionary;
    searchText : Observable<string>;
    selectedIndex : Observable<string>;
    index : DictionaryIndex;
    wordPage : WordPage;
    private loadedLink : string;
    indexes : Array<string>;

    selectedWord : Word = null;
    createWordLink : string;
    showCreateDialog : boolean = false;

    Languages = Languages;
    

    public searchForm = this.fb.group({
        query: [""]
    });

    constructor(private route: ActivatedRoute,
        private router: Router,
        public fb: FormBuilder,
        private dictionaryService: DictionaryService){
    }

    ngOnInit() {
        this.sub = this.route.params.subscribe(params => {
            this.id = params['id'];
            this.getDictionary();
        });

        this.searchText = this.route
            .queryParams
            .map(params => params['search'] || '');

        this.selectedIndex = this.route
            .queryParams
            .map(params => params['startWith'] || '');

        this.searchText.subscribe(
            (val) => {
                if (val !== "") {
                    this.searchForm.controls.query.setValue(val);
                    this.doSearch(val);
                }
            });
        this.selectedIndex.subscribe(
            (val) => {
                if (val !== "") this.getIndex(val);
            });
        
    }

    gotoIndex(index:DictionaryIndex){
        let navigationExtras: NavigationExtras = {
            queryParams: { 'startWith': index.link },
        };
        this.router.navigate(['/dictionary', this.id], navigationExtras);
        
    }

    gotoSearch(){
        let query = this.searchForm.controls.query.value;
        if (query == null || query.length < 0) return;

        let navigationExtras: NavigationExtras = {
            queryParams: { 'search': query }
        };
        this.router.navigate(['/dictionary', this.id], navigationExtras);
    }

    getIndex(index : string){
        this.isInSearch = false;
        this.isLoading = true;
        this.dictionaryService.getWordStartingWith(index)
            .subscribe(
                words => {
                    this.wordPage = words;
                    this.isLoading = false;
                },
                error => {
                this.errorMessage = <any>error;
            });
    }

    getDictionary() {
        this.isLoading = true;
        this.dictionaryService.getDictionary(this.id)
            .subscribe(
            dict => { 
                this.dictionary = dict;
                this.isLoading = false;
                this.getWords(this.dictionary.indexLink);
                this.createWordLink = dict.createWordLink;
            },
            error => {
                this.errorMessage = <any>error;
            });
    }
    
    getWords(link){
        this.isInSearch = false;
        this.isLoading = true;
        this.dictionaryService.getWords(link)
            .subscribe(
                words => {
                    this.wordPage = words;
                    this.isLoading = false;
                    this.loadedLink = link;
                },
                error => {
                this.errorMessage = <any>error;
            });
    }

    reloadPage(){
        this.getWords(this.loadedLink);
    }
    loadPage(link){
        this.getWords(link);
    }
    clearSearch(){
        this.searchForm.setValue({ query : ''});
        this.getWords(this.loadedLink);
    }

    doSearch(searchValue) {
        var searchValue = this.searchForm.controls.query.value;
        if(searchValue != null && searchValue.length > 0){
              this.isInSearch = true;
              this.isLoading = true; 
              this.dictionaryService.searchWords(this.dictionary.searchLink, searchValue)
              .subscribe(
                words => {
                    this.wordPage = words;
                    this.isLoading = false;
                },
                error => {
                this.errorMessage = <any>error;
            });
        }
    }

    addWord(){
        this.showCreateDialog = true;
    }
    onCreateClosed(created : boolean){
        this.showCreateDialog = false;
        this.selectedWord = null;
        if (created){
            this.reloadPage();
        }
    }

    editWord(word : Word){
        this.showCreateDialog = true;
        this.selectedWord = word;
    }

    deleteWord(word : Word){
        this.dictionaryService.deleteWord(word.deleteLink)
        .subscribe(r => {
            this.reloadPage();
        }, this.handlerError);
    }

    handlerError(error : any) {
        this.errorMessage = <any>error;
        this.isLoading = false;
    }
}
