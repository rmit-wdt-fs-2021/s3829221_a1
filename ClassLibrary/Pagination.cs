using System;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class Pagination<T>
    {
        private readonly List<T> _list;
        private readonly int _linesPerPage;
        private readonly int _pageCount;
        private int _currentPage;
        private int _firstIndex;
        
        public Pagination(List<T> list, int linesPerPage)
        {
            _list = list;
            _linesPerPage = linesPerPage;
            _pageCount = decimal.ToInt32(Math.Ceiling((decimal)_list.Count / _linesPerPage));
            _currentPage = 1;
            _firstIndex = 0;
        }


        public void Print()
        {
            if (_list.Count - _firstIndex >= _linesPerPage)
            {
                for (int i = _firstIndex; i <= _firstIndex + _linesPerPage - 1; i++)
                {
                    Console.WriteLine(_list[i]);
                }
            }

            else
            {
                for (int i = _firstIndex; i <= _list.Count - 1; i++)
                {
                    Console.WriteLine(_list[i]);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Page {0}/{1}", _currentPage, _pageCount);
            Console.WriteLine();
        }


        public void Next()
        {
            // Check if the current page is the last page
            if (_currentPage == _pageCount)
            {
                Console.WriteLine("This is already the last page.");
                Console.WriteLine();
            }

            else
            {
                _currentPage++;
                _firstIndex += _linesPerPage;
            }
        }


        public void Previous()
        {
            // Check if the current page is the first page
            if (_currentPage == 1)
            {
                Console.WriteLine("This is already the first page.");
                Console.WriteLine();
            }

            else
            {
                _currentPage--;
                _firstIndex -= _linesPerPage;
            }
        }
    }
}
